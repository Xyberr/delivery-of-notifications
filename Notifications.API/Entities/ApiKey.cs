using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Notifications.API.Entities;

public class ApiKey : BaseEntity
{
    [Required] 
    public string Key { get; set; } = null!;

    [Required] 
    public string Owner { get; set; } = null!;
    
    public string Desc { get; set; } = null!;
}