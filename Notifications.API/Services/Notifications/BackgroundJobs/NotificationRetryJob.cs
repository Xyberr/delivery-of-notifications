using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Quartz;

namespace Notifications.API.Services.Notifications.BackgroundJobs;

public class NotificationRetryJob(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationRetryJob> logger) : IJob
{
    private const int BatchSize = 100;

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var queuedStatusId = (long)DeliveryStatusCode.Queued;
        var pendingStatusId = (long)DeliveryStatusCode.Pending;

        var recipients = await db.MessageRecipients
            .Where(r => r.DeliveryStatusId == queuedStatusId)
            .OrderBy(r => r.Id)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        if (recipients.Count == 0)
            return;

        foreach (var recipient in recipients)
        {
            recipient.DeliveryStatusId = pendingStatusId;
            recipient.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(context.CancellationToken);

        try
        {
            await publish.Publish(
                new SendNotificationMessage
                {
                    RecipientIds = recipients.Select(r => r.Id).ToList()
                },
                context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка публикации batch уведомлений");

            foreach (var recipient in recipients)
            {
                recipient.DeliveryStatusId = queuedStatusId;
                recipient.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(context.CancellationToken);
        }
    }
}