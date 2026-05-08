namespace Notifications.API.DTO.Responses;

public class CreateMessageResponse
{
    public long MessageId { get; set; }
    public int RecipientsCount { get; set; }
}