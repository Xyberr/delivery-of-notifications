using Notifications.API.Entities.Enums;

namespace Notifications.API.Entities;

public class ContactType : BaseEntity, ISoftDeletable
{
    public string Name { get; set; } = null!; // email / sms / telegram
    public string Description { get; set; } = null!;
    
    public ContactTypeCode Code { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}