using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;

namespace Notifications.API.Consumers;

public class NotificationConsumer(
    AppDbContext db,
    ILogger<NotificationConsumer> logger)
    : IConsumer<SendNotificationMessage>
{
    private const int MaxRetryCount = 5;

    public async Task Consume(
        ConsumeContext<SendNotificationMessage> context)
    {
        var recipient = await db.MessageRecipients
            .FirstOrDefaultAsync(
                recipient =>
                    recipient.Id == context.Message.RecipientId,
                context.CancellationToken);

        if (recipient == null)
        {
            logger.LogWarning(
                "Получатель не найден. RecipientId: {RecipientId}",
                context.Message.RecipientId);

            return;
        }

        try
        {
            logger.LogInformation(
                "Отправка сообщения получателю {Recipient}",
                recipient.ContactData);

            // TODO: отправка

            recipient.DeliveryStatusId =
                (long)DeliveryStatusCode.Delivered;
        }
        catch (Exception exception)
        {
            recipient.RetryCount++;

            if (recipient.RetryCount >= MaxRetryCount)
            {
                recipient.DeliveryStatusId =
                    (long)DeliveryStatusCode.Failed;
            }
            else
            {
                recipient.DeliveryStatusId =
                    (long)DeliveryStatusCode.RetryScheduled;

                recipient.NextRetry =
                    DateTime.UtcNow.AddMinutes(5);
            }

            logger.LogError(
                exception,
                "Ошибка отправки уведомления {Recipient}",
                recipient.ContactData);
        }

        recipient.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(context.CancellationToken);
    }
}