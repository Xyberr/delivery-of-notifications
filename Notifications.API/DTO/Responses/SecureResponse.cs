namespace Notifications.API.DTO.Responses;

public class SecureResponse
{
    public string Message { get; set; } = null!;
    public string? Owner { get; set; }
    public string? Desc { get; set; }
    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
}