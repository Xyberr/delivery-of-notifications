namespace Notifications.API.Entities;

public class Message : BaseEntity
{
    public string Subject { get; set; } = null!;
    public string MessageBody { get; set; } = null!;
    public int StorageTimeAfterSendingInHours { get; set; }

    public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
    public ICollection<MessageAttachment> Attachments { get; set; } = new List<MessageAttachment>();
}