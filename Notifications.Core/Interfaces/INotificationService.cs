using Notifications.Contracts.Requests;

namespace Notifications.Core.Interfaces;

public interface INotificationService
{
    Task<Guid> CreateAsync(CreateNotificationRequest request);
}