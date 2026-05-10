using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;

namespace Notifications.API.Services.BackgroundServices.NotificationRetryBackgroundService;

public class NotificationRetryBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationRetryBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var db = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var publish = scope.ServiceProvider
                    .GetRequiredService<IPublishEndpoint>();

                var recipients = await db.MessageRecipients
                    .Where(recipient =>
                        recipient.DeliveryStatusId ==
                            (long)DeliveryStatusCode.Queued
                        ||
                        recipient.DeliveryStatusId ==
                            (long)DeliveryStatusCode.Failed)
                    .Where(recipient =>
                        recipient.NextRetry == null
                        || recipient.NextRetry <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var recipient in recipients)
                {
                    try
                    {
                        recipient.DeliveryStatusId =
                            (long)DeliveryStatusCode.Pending;

                        recipient.UpdatedAt = DateTime.UtcNow;

                        await db.SaveChangesAsync(stoppingToken);

                        await publish.Publish(
                            new SendNotificationMessage
                            {
                                RecipientId = recipient.Id
                            },
                            stoppingToken);
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(
                            exception,
                            "Не удалось перепубликовать recipient {RecipientId}",
                            recipient.Id);

                        recipient.DeliveryStatusId =
                            (long)DeliveryStatusCode.Failed;

                        recipient.NextRetry =
                            DateTime.UtcNow.AddMinutes(5);

                        recipient.UpdatedAt = DateTime.UtcNow;

                        await db.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Ошибка retry background service");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(30),
                stoppingToken);
        }
    }
}