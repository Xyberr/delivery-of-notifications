using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;

namespace Notifications.API.Service.MessageService;

public class MessageService(AppDbContext db) : IMessageService
{
    public async Task<CreateMessageResponse> CreateAsync(CreateMessageRequest request)
    {
        var pendingStatus = await db.DeliveryStatuses
            .FirstOrDefaultAsync(x => x.Code == (int)DeliveryStatusCode.Pending);

        if (pendingStatus == null)
            throw new Exception("Статус доставки 'Pending' не найден");

        if (request.Recipients == null || request.Recipients.Count == 0)
            throw new Exception("Список получателей пуст");

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
                DeliveryStatusId = pendingStatus.Id,
                RetryCount = 0,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        db.Messages.Add(message);
        await db.SaveChangesAsync();

        return new CreateMessageResponse
        {
            MessageId = message.Id,
            RecipientsCount = message.Recipients.Count
        };
    }

    public async Task<MessageResponse?> GetByIdAsync(long id)
    {
        var message = await db.Messages
            .Include(x => x.Recipients)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (message == null)
            return null;

        return Map(message);
    }

    public async Task<List<MessageResponse>> GetAllAsync(int page, int pageSize)
    {
        var messages = await db.Messages
            .Include(x => x.Recipients)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return messages.Select(Map).ToList();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var message = await db.Messages.FindAsync(id);

        if (message == null)
            return false;

        db.Messages.Remove(message);
        await db.SaveChangesAsync();

        return true;
    }

    private static MessageResponse Map(Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            Subject = message.Subject,
            MessageBody = message.MessageBody,
            StorageTimeAfterSendingInHours = message.StorageTimeAfterSendingInHours,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            Recipients = message.Recipients.Select(r => new RecipientResponse
            {
                Id = r.Id,
                ContactTypeId = r.ContactTypeId,
                ContactData = r.ContactData,
                DeliveryStatusId = r.DeliveryStatusId
            }).ToList()
        };
    }
}