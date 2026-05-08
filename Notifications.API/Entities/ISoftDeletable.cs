using System.ComponentModel.DataAnnotations.Schema;

namespace Notifications.API.Entities;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime? DeletedAt { get; set; }
}