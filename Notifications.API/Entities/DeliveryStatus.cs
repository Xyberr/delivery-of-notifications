namespace Notifications.API.Entities;

public class DeliveryStatus : BaseEntity
{
    public string Name { get; set; } = null!; // Pending / Sent / Failed
    public string Description { get; set; } = null!;
    public int Code { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}