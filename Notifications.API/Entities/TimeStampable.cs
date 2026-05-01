using System.ComponentModel.DataAnnotations.Schema;

namespace Notifications.API.Entities;

public interface ITimeStampable
{
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }
    
    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; }
}