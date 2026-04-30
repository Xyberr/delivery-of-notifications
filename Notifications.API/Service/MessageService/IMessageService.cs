using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;

namespace Notifications.API.Service.MessageService;

public interface IMessageService
{
    Task<CreateMessageResponse> CreateAsync(CreateMessageRequest request);
}