using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;

namespace Notifications.API.Service.MessageService;

public interface IMessageService
{
    Task<Result<CreateMessageResponse>> CreateAsync(CreateMessageRequest request, CancellationToken cancellationToken);

    Task<MessageResponse?> GetByIdAsync(long id, CancellationToken cancellationToken);

    Task<PagedResult<MessageResponse>> GetListAsync(int? page, int? pageSize, string? sortBy, bool desc, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
}