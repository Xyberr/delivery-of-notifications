namespace Notifications.API.Entities;

public class Recipient : BaseEntity, ISoftDeletable
{
    public long MessageId { get; set; }
    public Message Message { get; set; } = null!;

    public long ContactTypeId { get; set; }
    public ContactType ContactType { get; set; } = null!;

    public string ContactData { get; set; } = null!;

    public long DeliveryStatusId { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; } = null!;

    public int RetryCount { get; set; }
    public DateTime? NextRetry { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}