using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Responses;
using Notifications.API.DTO.Responses.Components;

namespace Notifications.API.Service.MessageService;

public partial class MessageService
{
    public async Task<IEnumerable<MessageResponse>> GetListAsync(CancellationToken cancellationToken)
    {
        return await db.Messages
            .AsNoTracking()
            .OrderByDescending(message => message.CreatedAt)
            .Select(message => new MessageResponse
            {
                Id = message.Id,
                Subject = message.Subject,
                MessageBody = message.MessageBody,
                StorageTimeAfterSendingInHours = message.StorageTimeAfterSendingInHours,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt,
                Recipients = message.Recipients
                    .Select(recipient => new RecipientResponseData
                    {
                        Id = recipient.Id,
                        ContactData = recipient.ContactData,
                        ContactTypeId = recipient.ContactTypeId,
                        DeliveryStatusId = recipient.DeliveryStatusId
                    })
            })
            .ToListAsync(cancellationToken);
    }
}