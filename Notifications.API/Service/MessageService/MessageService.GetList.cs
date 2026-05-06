using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Responses;
using Notifications.API.DTO.Responses.Components;

namespace Notifications.API.Service.MessageService;

public partial class MessageService
{
    public async Task<IEnumerable<MessageResponse>> GetListAsync(int? page, int? pageSize, string? sortBy, bool desc, CancellationToken cancellationToken)
    {
        var query = db.Messages.AsQueryable();
        var pageNumber = Math.Max(page ?? 1, 1);
        var size = Math.Max(pageSize ?? 10, 1);

        query = sortBy switch
        {
            "createdAt" => desc
                ? query.OrderByDescending(message => message.CreatedAt)
                : query.OrderBy(message => message.CreatedAt),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        if (page.HasValue && pageSize.HasValue)
        {
            query = query
                .Skip((pageNumber - 1) * size)
                .Take(size);
        }

        return await query
            .Select(message => new MessageResponse
            {
                Id = message.Id,
                Subject = message.Subject,
                MessageBody = message.MessageBody,
                StorageTimeAfterSendingInHours = message.StorageTimeAfterSendingInHours,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt,
                Recipients = message.Recipients.Select(recipient => new RecipientData
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