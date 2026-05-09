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
        var messageId = context.Message.MessageId;

        logger.LogInformation(
            "Начата обработка сообщения {MessageId}",
            messageId);

        var message = await db.Messages
            .Include(x => x.Recipients)
            .FirstOrDefaultAsync(
                x => x.Id == messageId,
                context.CancellationToken);

        if (message == null)
        {
            logger.LogWarning(
                "Сообщение {MessageId} не найдено",
                messageId);

            return;
        }

        var pendingStatusId = await db.DeliveryStatuses
            .Where(x => x.Code == DeliveryStatusCode.Pending)
            .Select(x => x.Id)
            .FirstAsync(context.CancellationToken);

        var deliveredStatusId = await db.DeliveryStatuses
            .Where(x => x.Code == DeliveryStatusCode.Delivered)
            .Select(x => x.Id)
            .FirstAsync(context.CancellationToken);

        foreach (var recipient in message.Recipients)
        {
            recipient.DeliveryStatusId = pendingStatusId;
            recipient.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(context.CancellationToken);

        try
        {
            foreach (var recipient in message.Recipients)
            {
                logger.LogInformation(
                    "Отправка сообщения {MessageId} получателю {RecipientId}",
                    message.Id,
                    recipient.Id);

                await Task.Delay(
                    1000,
                    context.CancellationToken);

                recipient.DeliveryStatusId = deliveredStatusId;
                recipient.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(context.CancellationToken);

            logger.LogInformation(
                "Сообщение {MessageId} успешно обработано",
                messageId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка при обработке сообщения {MessageId}",
                messageId);

            throw;
        }
    }
}