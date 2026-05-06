namespace Notifications.API.Entities;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = null!;
    public string StoragePath { get; set; } = null!;

    public long MessageId { get; set; }
    public Message Message { get; set; } = null!;
}