using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Notifications.API.Services.DeliveryStatusProvider;
using Notifications.API.Services.Notifications;

namespace Notifications.API.Consumers;

public class NotificationConsumer(
    AppDbContext db,
    ILogger<NotificationConsumer> logger,
    INotificationSender sender,
    IDeliveryStatusProvider deliveryStatusProvider)
    : IConsumer<SendNotificationMessage>
{
    private const int MaxRetryCount = 5;

    public async Task Consume(
        ConsumeContext<SendNotificationMessage> context)
    {
        var recipients = await db.MessageRecipients
            .Include(recipient => recipient.Message)
            .Where(recipient =>
                context.Message.RecipientIds.Contains(recipient.Id))
            .ToListAsync(context.CancellationToken);

        var deliveredStatusId =
            await deliveryStatusProvider.GetStatusIdAsync(
                DeliveryStatusCode.Delivered,
                context.CancellationToken);

        var retryScheduledStatusId =
            await deliveryStatusProvider.GetStatusIdAsync(
                DeliveryStatusCode.RetryScheduled,
                context.CancellationToken);

        var failedStatusId =
            await deliveryStatusProvider.GetStatusIdAsync(
                DeliveryStatusCode.Failed,
                context.CancellationToken);

        foreach (var recipient in recipients)
        {
            try
            {
                await sender.SendAsync(
                    recipient,
                    recipient.Message.Subject,
                    recipient.Message.MessageBody,
                    context.CancellationToken);

                recipient.DeliveryStatusId =
                    deliveredStatusId;
            }
            catch (Exception exception)
            {
                recipient.RetryCount++;

                if (recipient.RetryCount >= MaxRetryCount)
                {
                    recipient.DeliveryStatusId =
                        failedStatusId;
                }
                else
                {
                    recipient.DeliveryStatusId =
                        retryScheduledStatusId;

                    recipient.NextRetry =
                        DateTime.UtcNow.AddMinutes(5);
                }

                logger.LogError(
                    exception,
                    "Ошибка отправки уведомления {Recipient}",
                    recipient.ContactData);
            }

            recipient.UpdatedAt =
                DateTime.UtcNow;
        }

        await db.SaveChangesAsync(
            context.CancellationToken);
    }
}