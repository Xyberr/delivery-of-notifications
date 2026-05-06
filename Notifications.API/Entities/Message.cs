namespace Notifications.API.Entities;

public class Message : BaseEntity
{
    public string Subject { get; set; } = null!;
    public string MessageBody { get; set; } = null!;
    public int StorageTimeAfterSendingInHours { get; set; }

    public ICollection<Recipient> Recipients { get; set; } = [];
    public ICollection<Attachment> Attachments { get; set; } = [];
}