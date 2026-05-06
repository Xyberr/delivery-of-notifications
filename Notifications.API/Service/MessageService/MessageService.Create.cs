using Microsoft.EntityFrameworkCore;
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
    if (request.Recipients == null || request.Recipients.Count == 0)
        return Result<CreateMessageResponse>.Failure("Список получателей не может быть пустым");

    var now = DateTime.UtcNow;

    var pendingStatusId = await db.DeliveryStatuses
        .Where(status => status.Code == DeliveryStatusCode.Pending)
        .Select(status => (long?)status.Id)
        .FirstOrDefaultAsync(cancellationToken);

    if (pendingStatusId == null)
        return Result<CreateMessageResponse>.Failure("Статус Pending не найден");

    var contactTypeIds = request.Recipients
        .Select(r => r.ContactTypeId)
        .Distinct()
        .ToList();

    var existingIds = await db.ContactTypes
        .Where(ct => contactTypeIds.Contains(ct.Id))
        .Select(ct => ct.Id)
        .ToListAsync(cancellationToken);

    var invalidIds = contactTypeIds.Except(existingIds).ToList();

    if (invalidIds.Any())
        return Result<CreateMessageResponse>.Failure($"Неверные ContactTypeId: {string.Join(", ", invalidIds)}");

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
                DeliveryStatusId = pendingStatusId.Value,
                RetryCount = 0,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList()
    };

    db.Messages.Add(message);
    await db.SaveChangesAsync(cancellationToken);

    return Result<CreateMessageResponse>.Success(new CreateMessageResponse
    {
        MessageId = message.Id,
        RecipientsCount = message.Recipients.Count
    });
}
}