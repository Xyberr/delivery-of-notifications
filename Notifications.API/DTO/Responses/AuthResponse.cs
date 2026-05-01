namespace Notifications.API.DTO.Responses;

public record AuthResponse
{
    public string Owner { get; init; }
    public string Desc { get; init; }
    public string CreateBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}