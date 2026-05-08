using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public partial class AuthService
{
    private async Task<ApiKey?> GetApiKeyAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            return await db.ApiKeys
                .AsNoTracking()
                .FirstOrDefaultAsync(api => api.Key == key, cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}