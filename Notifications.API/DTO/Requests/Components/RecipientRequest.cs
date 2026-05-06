namespace Notifications.API.DTO.Components;

public class RecipientRequest
{
    public long ContactTypeId { get; set; }
    public string ContactData { get; set; } = null!;
}