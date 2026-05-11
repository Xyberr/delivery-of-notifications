using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;
using Notifications.API.Services.Notifications;

namespace Notifications.API.Consumers;

public class NotificationConsumer(
    AppDbContext db,
    ILogger<NotificationConsumer> logger,
    INotificationSender sender)
    : IConsumer<SendNotificationMessage>
{
    private const int MaxRetryCount = 5;

    public async Task Consume(ConsumeContext<SendNotificationMessage> context)
    {
        var recipients = await db.MessageRecipients
            .Include(recipient => recipient.Message)
            .Where(recipient => context.Message.RecipientIds.Contains(recipient.Id))
            .ToListAsync(context.CancellationToken);

        foreach (var recipient in recipients)
        {
            try
            {
                await sender.SendAsync(
                    recipient,
                    recipient.Message.Subject,
                    recipient.Message.MessageBody,
                    context.CancellationToken);

                recipient.DeliveryStatusId = (long)DeliveryStatusCode.Delivered;
            }
            catch (Exception ex)
            {
                recipient.RetryCount++;

                if (recipient.RetryCount >= MaxRetryCount)
                    recipient.DeliveryStatusId = (long)DeliveryStatusCode.Failed;
                else
                    recipient.DeliveryStatusId = (long)DeliveryStatusCode.Queued;

                logger.LogError(ex,
                    "Ошибка отправки уведомления {Recipient}",
                    recipient.ContactData);
            }

            recipient.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(context.CancellationToken);
    }
}