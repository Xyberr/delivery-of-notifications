using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities;

public abstract class BaseEntity : ITimeStampable
{
    [Key]
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}