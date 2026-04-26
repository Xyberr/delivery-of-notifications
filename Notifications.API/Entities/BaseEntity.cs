using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities;

public abstract class BaseEntity : TimeStampable
{
    [Key]
    public long Id { get; set; }
}