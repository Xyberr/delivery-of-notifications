using Notifications.API.DTO.Responses.Components;

namespace Notifications.API.DTO.Responses;

public class MessageResponse
{
    public long Id { get; set; }
    public string Subject { get; set; } = null!;
    public string MessageBody { get; set; } = null!;
    public int StorageTimeAfterSendingInHours { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public IEnumerable<RecipientResponseData> Recipients { get; set; } = [];
}