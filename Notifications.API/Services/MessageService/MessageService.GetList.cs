using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Responses;
using Notifications.API.DTO.Responses.Components;
using Notifications.API.Entities;

namespace Notifications.API.Service.MessageService;

public partial class MessageService
{
    public async Task<PagedResult<MessageResponse>> GetListAsync(
        int? page,
        int? pageSize,
        string? sortBy,
        bool desc,
        CancellationToken cancellationToken)
    {
        var query = db.Messages.AsQueryable();

        // сортировка
        query = sortBy switch
        {
            "createdAt" => desc
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        if (page.HasValue && pageSize.HasValue)
        {
            query = query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        var items = await query
            .Select(message => new MessageResponse
            {
                Id = message.Id,
                Subject = message.Subject,
                MessageBody = message.MessageBody,
                StorageTimeAfterSendingInHours = message.StorageTimeAfterSendingInHours,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt,
                Recipients = message.Recipients.Select(recipient => new RecipientResponseData
                {
                    Id = recipient.Id,
                    ContactData = recipient.ContactData,
                    ContactTypeId = recipient.ContactTypeId,
                    DeliveryStatusId = recipient.DeliveryStatusId
                })
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<MessageResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page ?? 1,
            PageSize = pageSize ?? totalCount
        };
    }
}