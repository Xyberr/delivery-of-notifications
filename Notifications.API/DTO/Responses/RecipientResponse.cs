namespace Notifications.API.DTO.Responses;

public class RecipientResponse
{
    public long Id { get; set; }
    public long ContactTypeId { get; set; }
    public string ContactData { get; set; } = null!;
    public long DeliveryStatusId { get; set; }
}