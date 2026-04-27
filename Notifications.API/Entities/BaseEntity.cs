using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }
}