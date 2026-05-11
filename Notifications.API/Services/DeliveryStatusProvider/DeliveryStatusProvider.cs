using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities.Enums;
using Notifications.API.Persistence;

namespace Notifications.API.Services.DeliveryStatusProvider;

public class DeliveryStatusProvider(AppDbContext db) : IDeliveryStatusProvider
{
    private readonly Dictionary<DeliveryStatusCode, long> _cache = [];

    public async Task<long> GetStatusIdAsync(DeliveryStatusCode code, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(code, out var cachedId))
        {
            return cachedId;
        }

        var statusId = await db.DeliveryStatuses
            .Where(status => status.Code == code)
            .Select(status => status.Id)
            .FirstAsync(cancellationToken);

        _cache[code] = statusId;

        return statusId;
    }
}