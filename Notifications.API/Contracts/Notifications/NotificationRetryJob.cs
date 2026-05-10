
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Quartz;

namespace Notifications.API.Contracts.Notifications;
public class NotificationRetryJob(IServiceScopeFactory scopeFactory, ILogger<NotificationRetryJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var recipients = await db.MessageRecipients
            .Where(recipient =>
                (recipient.DeliveryStatusId == (long)DeliveryStatusCode.Queued ||
                 recipient.DeliveryStatusId == (long)DeliveryStatusCode.Failed) &&
                (recipient.NextRetry == null || recipient.NextRetry <= DateTime.UtcNow))
            .ToListAsync(context.CancellationToken);

        foreach (var recipient in recipients)
        {
            try
            {
                recipient.DeliveryStatusId = (long)DeliveryStatusCode.Pending;
                recipient.UpdatedAt = DateTime.UtcNow;

                await db.SaveChangesAsync(context.CancellationToken);

                await publish.Publish(
                    new SendNotificationMessage
                    {
                        RecipientId = recipient.Id
                    },
                    context.CancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Retry failed for {Id}", recipient.Id);

                recipient.DeliveryStatusId = (long)DeliveryStatusCode.Failed;
                recipient.NextRetry = DateTime.UtcNow.AddMinutes(5);
                recipient.UpdatedAt = DateTime.UtcNow;

                await db.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}