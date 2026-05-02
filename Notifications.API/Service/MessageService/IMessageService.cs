using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;

namespace Notifications.API.Service.MessageService;

public interface IMessageService
{
    Task<CreateMessageResponse> CreateAsync(CreateMessageRequest request);

    Task<MessageResponse?> GetByIdAsync(long id);

    Task<List<MessageResponse>> GetAllAsync(int page, int pageSize);

    Task<bool> DeleteAsync(long id);
}