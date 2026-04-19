using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Notifications.API.Entities;

public class ApiKey
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Key { get; set; } = null!;

    [Required]
    public string Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}