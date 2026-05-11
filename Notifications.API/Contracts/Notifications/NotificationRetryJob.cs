using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Notifications.API.Services.DeliveryStatusProvider;
using Quartz;

namespace Notifications.API.Contracts.Notifications;

public class NotificationRetryJob(IServiceScopeFactory scopeFactory, ILogger<NotificationRetryJob> logger) : IJob
{
    private const int BatchSize = 100;

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var publish = scope.ServiceProvider
            .GetRequiredService<IPublishEndpoint>();

        var deliveryStatusProvider = scope.ServiceProvider
            .GetRequiredService<IDeliveryStatusProvider>();

        var queuedStatusId = await deliveryStatusProvider
            .GetStatusIdAsync(DeliveryStatusCode.Queued, context.CancellationToken);

        var retryScheduledStatusId = await deliveryStatusProvider
            .GetStatusIdAsync(DeliveryStatusCode.RetryScheduled, context.CancellationToken);

        var pendingStatusId = await deliveryStatusProvider
            .GetStatusIdAsync(DeliveryStatusCode.Pending, context.CancellationToken);

        var stuckThreshold =
            DateTime.UtcNow.AddMinutes(-5);

        var recipients = await db.MessageRecipients
            .Where(recipient =>
                recipient.DeliveryStatusId == queuedStatusId
                || recipient.DeliveryStatusId == retryScheduledStatusId
                || (recipient.DeliveryStatusId == pendingStatusId
                    && recipient.UpdatedAt < stuckThreshold))
            .Where(recipient =>
                recipient.NextRetry == null
                || recipient.NextRetry <= DateTime.UtcNow)
            .OrderBy(recipient => recipient.Id)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        if (recipients.Count == 0)
        {
            return;
        }

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
                    RecipientIds = recipients
                        .Select(recipient => recipient.Id)
                        .ToList()
                },
                context.CancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Ошибка публикации batch уведомлений");

            foreach (var recipient in recipients)
            {
                recipient.DeliveryStatusId =
                    retryScheduledStatusId;

                recipient.NextRetry =
                    DateTime.UtcNow.AddMinutes(5);

                recipient.UpdatedAt =
                    DateTime.UtcNow;
            }

            await db.SaveChangesAsync(context.CancellationToken);
        }
    }
}