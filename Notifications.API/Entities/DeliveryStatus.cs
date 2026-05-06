using Notifications.API.Entities.Enums;

namespace Notifications.API.Entities;

public class DeliveryStatus : BaseEntity, ISoftDeletable
{
    public string Name { get; set; } = null!; // Pending / Sent / Failed
    public string Description { get; set; } = null!;
    public DeliveryStatusCode Code { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}