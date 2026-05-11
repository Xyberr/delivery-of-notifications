using Notifications.API.Entities;

namespace Notifications.API.Services.Notifications;

public class FakeNotificationSender(
    ILogger<FakeNotificationSender> logger)
    : INotificationSender
{
    public async Task SendAsync(
        Recipient recipient,
        string subject,
        string messageBody,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            """
            MOCK SEND:
            Recipient: {Recipient}
            Subject: {Subject}
            Message: {Message}
            """,
            recipient.ContactData,
            subject,
            messageBody);

        await Task.Delay(
            TimeSpan.FromMilliseconds(300),
            cancellationToken);
    }
}