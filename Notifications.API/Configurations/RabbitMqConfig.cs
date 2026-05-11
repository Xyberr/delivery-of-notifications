using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities;


public record RabbitMqConfig : IConfigurable
{
    public static string SectionName => "RabbitMq";
    
    [Required]
    public Uri Host { get; init; } = null!;
    
    public string Username { get; init; } = null!;
    
    public string Password { get; init; } = null!;
}