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
        var message = await db.Messages
            .Include(message => message.Recipients)
            .FirstOrDefaultAsync(
                message => message.Id == context.Message.MessageId);

        if (message == null)
        {
            logger.LogWarning(
                "Сообщение не найдено. MessageId: {MessageId}",
                context.Message.MessageId);

            return;
        }

        foreach (var recipient in message.Recipients)
        {
            try
            {
                logger.LogInformation(
                    "Отправка сообщения в {Recipient}",
                    recipient.ContactData);

                recipient.DeliveryStatusId =
                    (long)DeliveryStatusCode.Delivered;
            }
            catch (Exception exception)
            {
                recipient.RetryCount++;

                recipient.DeliveryStatusId =
                    (long)DeliveryStatusCode.Failed;

                logger.LogError(
                    exception,
                    "Не удалось отправить уведомление по адресу {Recipient}",
                    recipient.ContactData);

                throw;
            }
        }

        await db.SaveChangesAsync(context.CancellationToken);
    }
}