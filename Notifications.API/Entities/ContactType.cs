namespace Notifications.API.Entities;

public class ContactType : BaseEntity
{
    public string Name { get; set; } = null!; // email / sms / telegram
    public string Description { get; set; } = null!;
    public int Code { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}