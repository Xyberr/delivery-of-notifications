namespace Notifications.Contracts.DTO.Responses;

public class CreateNotificationResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
}