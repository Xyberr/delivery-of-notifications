using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.DTO;
using Notifications.API.Entities;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;

namespace Notifications.API.Service.MessageService;

public class MessageService(AppDbContext db, ILogger<MessageService> logger) : IMessageService
{
    public async Task<CreateMessageResponse> CreateAsync(CreateMessageRequest request)
    {
        try
        {
            var pendingStatus = await GetPendingStatusAsync();

            await ValidateRecipientsAsync(request.Recipients);

            var message = BuildMessage(request, pendingStatus.Id);

            db.Messages.Add(message);
            await db.SaveChangesAsync();

            return new CreateMessageResponse
            {
                MessageId = message.Id,
                RecipientsCount = message.Recipients.Count
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Ошибка базы данных при создании сообщения");
            throw new Exception("Ошибка при сохранении сообщения в базе данных");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Непредвиденная ошибка при создании сообщения");
            throw;
        }
    }

    private async Task<DeliveryStatus> GetPendingStatusAsync()
    {
        var status = await db.DeliveryStatuses
            .FirstOrDefaultAsync(x => x.Code == (int)DeliveryStatusCode.Pending);

        if (status == null)
            throw new Exception("Статус доставки 'Pending' не найден");

        return status;
    }

    private async Task ValidateRecipientsAsync(List<RecipientDto> recipients)
    {
        if (recipients == null || recipients.Count == 0)
            throw new Exception("Список получателей не может быть пустым");

        var ids = recipients
            .Select(r => r.ContactTypeId)
            .Distinct()
            .ToList();

        var existingIds = await db.ContactTypes
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        var invalidIds = ids.Except(existingIds).ToList();

        if (invalidIds.Any())
            throw new Exception($"Некорректные ContactTypeId: {string.Join(", ", invalidIds)}");
    }

    private Message BuildMessage(CreateMessageRequest request, long statusId)
    {
        var now = DateTime.UtcNow;

        var message = new Message
        {
            Subject = request.Subject,
            MessageBody = request.MessageBody,
            StorageTimeAfterSendingInHours = request.StorageTimeAfterSendingInHours,
            CreatedAt = now,
            UpdatedAt = now,
            Recipients = new List<MessageRecipient>()
        };

        foreach (var r in request.Recipients)
        {
            message.Recipients.Add(new MessageRecipient
            {
                ContactTypeId = r.ContactTypeId,
                ContactData = r.ContactData,
                DeliveryStatusId = statusId,
                RetryCount = 0,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        return message;
    }
}