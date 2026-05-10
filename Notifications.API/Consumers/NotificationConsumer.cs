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

            recipient.DeliveryStatusId =
                (long)DeliveryStatusCode.Delivered;
        }
        catch (Exception exception)
        {
            recipient.RetryCount++;
            recipient.NextRetry = DateTime.UtcNow.AddMinutes(5);
            recipient.DeliveryStatusId = (long)DeliveryStatusCode.Failed;

            logger.LogError(exception, "Ошибка отправки уведомления {Recipient}", recipient.ContactData);

            await db.SaveChangesAsync(context.CancellationToken);

            return;
        }

        await db.SaveChangesAsync(context.CancellationToken);
    }
}