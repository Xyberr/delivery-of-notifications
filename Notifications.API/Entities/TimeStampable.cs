namespace Notifications.API.Entities;

public abstract class TimeStampable
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}