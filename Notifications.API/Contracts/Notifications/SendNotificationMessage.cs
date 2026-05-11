namespace Notifications.API.Contracts.Notifications;

public class SendNotificationMessage
{
    public ICollection<long> RecipientIds { get; set; } = [];
}