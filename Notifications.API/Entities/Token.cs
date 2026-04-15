using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Notifications.API.Entities;

[Table("Tokens")]
public class Token
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Value { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}