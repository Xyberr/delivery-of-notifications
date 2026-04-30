namespace Notifications.API.DTO;

public class RecipientDto
{
    public long ContactTypeId { get; set; }
    public string ContactData { get; set; } = null!;
}