using Notifications.API.Entities;

namespace Notifications.API.Services.Notifications;

public interface INotificationSender
{
    Task SendAsync(Recipient recipient, string subject, string messageBody, CancellationToken cancellationToken);
}