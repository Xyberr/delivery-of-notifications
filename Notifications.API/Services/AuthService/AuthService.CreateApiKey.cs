using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public partial class AuthService
{
    public async Task<string> CreateApiKeyAsync(string owner, string desc, string createdBy, CancellationToken cancellationToken)
    {
        var key = GenerateKey();

        var entity = new ApiKey
        {
            Key = key,
            Owner = owner,
            Desc = desc,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.ApiKeys.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        return key;
    }
}