using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;
using Notifications.API.Entities.Enums;

namespace Notifications.API.Service.MessageService;

public partial class MessageService
{
    public async Task<Result<CreateMessageResponse>> CreateAsync(
        CreateMessageRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Recipients is not { Count: >= 1 })
            return Result<CreateMessageResponse>
                .Failure("Список получателей не может быть пустым");

        var now = DateTime.UtcNow;

        var pendingStatusId = await db.DeliveryStatuses
            .Where(status => status.Code == DeliveryStatusCode.Pending)
            .Select(status => status.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (pendingStatusId == default)
            return Result<CreateMessageResponse>
                .Failure("Статус Pending не найден");

        var contactTypeIds = request.Recipients
            .Select(recipient => recipient.ContactTypeId)
            .Distinct()
            .ToList();

        var existingIds = await db.ContactTypes
            .Where(contactType => contactTypeIds.Contains(contactType.Id))
            .Select(contactType => contactType.Id)
            .ToListAsync(cancellationToken);

        var invalidIds = contactTypeIds
            .Except(existingIds)
            .ToList();

        if (invalidIds.Count != 0)
        {
            return Result<CreateMessageResponse>
                .Failure($"Неподдерживаемые ContactTypeId: {string.Join(", ", invalidIds)}");
        }

        var message = new Message
        {
            Subject = request.Subject,
            MessageBody = request.MessageBody,
            StorageTimeAfterSendingInHours = request.StorageTimeAfterSendingInHours,
            CreatedAt = now,
            UpdatedAt = now,
            Recipients = request.Recipients
                .Select(recipient => new Recipient
                {
                    ContactTypeId = recipient.ContactTypeId,
                    ContactData = recipient.ContactData,
                    DeliveryStatusId = pendingStatusId,
                    RetryCount = 0,
                    CreatedAt = now,
                    UpdatedAt = now
                })
                .ToList()
        };

        db.Messages.Add(message);

        await db.SaveChangesAsync(cancellationToken);

        try
        {
            await publish.Publish(
                new SendNotificationMessage
                {
                    MessageId = message.Id
                },
                cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Не удалось опубликовать уведомление. MessageId: {MessageId}", message.Id);
        }

        return Result<CreateMessageResponse>.Success(
            new CreateMessageResponse
            {
                MessageId = message.Id,
                RecipientsCount = message.Recipients.Count
            });
    }
}