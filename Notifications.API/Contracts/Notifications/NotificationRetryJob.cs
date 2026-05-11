using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Quartz;

namespace Notifications.API.Contracts.Notifications;

public class NotificationRetryJob(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationRetryJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var stuckThreshold = DateTime.UtcNow.AddMinutes(-5);

        var recipients = await db.MessageRecipients
            .Where(recipient =>
                recipient.DeliveryStatusId == (long)DeliveryStatusCode.Queued
                || recipient.DeliveryStatusId == (long)DeliveryStatusCode.RetryScheduled
                || (recipient.DeliveryStatusId == (long)DeliveryStatusCode.Pending
                    && recipient.UpdatedAt < stuckThreshold))
            .Where(recipient =>
                recipient.NextRetry == null
                || recipient.NextRetry <= DateTime.UtcNow)
            .ToListAsync(context.CancellationToken);

        foreach (var recipient in recipients)
        {
            recipient.DeliveryStatusId =
                (long)DeliveryStatusCode.Pending;

            recipient.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(context.CancellationToken);

        foreach (var recipient in recipients)
        {
            try
            {
                await publish.Publish(
                    new SendNotificationMessage
                    {
                        RecipientId = recipient.Id
                    },
                    context.CancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Не удалось отправить recipient {RecipientId} в RabbitMQ",
                    recipient.Id);

                recipient.DeliveryStatusId =
                    (long)DeliveryStatusCode.RetryScheduled;

                recipient.NextRetry =
                    DateTime.UtcNow.AddMinutes(5);

                recipient.UpdatedAt = DateTime.UtcNow;
            }
        }

        await db.SaveChangesAsync(context.CancellationToken);
    }
}