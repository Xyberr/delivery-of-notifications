namespace Notifications.API.DTO.Responses.Components;

public class RecipientData
{
    public long Id { get; set; }
    public long ContactTypeId { get; set; }
    public string ContactData { get; set; } = null!;
    public long DeliveryStatusId { get; set; }
}