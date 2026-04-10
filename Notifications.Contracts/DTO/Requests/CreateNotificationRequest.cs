using System.ComponentModel.DataAnnotations;

namespace Notifications.Contracts.Requests;

public class CreateNotificationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}