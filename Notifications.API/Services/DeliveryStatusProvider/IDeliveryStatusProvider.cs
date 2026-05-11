using Notifications.API.Entities.Enums;

namespace Notifications.API.Services.DeliveryStatusProvider;

public interface IDeliveryStatusProvider
{
    Task<long> GetStatusIdAsync(DeliveryStatusCode code, CancellationToken cancellationToken);
}