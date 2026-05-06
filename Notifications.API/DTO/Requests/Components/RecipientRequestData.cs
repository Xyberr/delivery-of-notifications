namespace Notifications.API.DTO.Components;

public class RecipientRequestData
{
    public long ContactTypeId { get; set; }
    public string ContactData { get; set; } = null!;
}