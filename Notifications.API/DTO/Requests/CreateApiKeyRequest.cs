namespace Notifications.API.DTO.Requests;

public class CreateApiKeyRequest
{
    public string Owner { get; set; } = null!;
    public string Desc { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
}