namespace Notifications.API.Entities;

public class MessageAttachment : BaseEntity
{
    public string FileName { get; set; } = null!;
    public string StoragePath { get; set; } = null!;

    public long MessageId { get; set; }
    public Message Message { get; set; } = null!;
}