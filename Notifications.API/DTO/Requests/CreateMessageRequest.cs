using Notifications.API.DTO.Components;

namespace Notifications.API.DTO.Requests;

public class CreateMessageRequest
{
    public string Subject { get; set; } = null!;
    public string MessageBody { get; set; } = null!;

    public int StorageTimeAfterSendingInHours { get; set; }

    public List<RecipientRequestData> Recipients { get; set; } = new();
}