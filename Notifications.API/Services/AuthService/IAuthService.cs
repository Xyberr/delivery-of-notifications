using Notifications.API.DTO.Responses;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<string> CreateApiKeyAsync(string owner, string desc, string createBy, CancellationToken cancellationToken);
    Task<AuthResponse?> LoginAsync(HttpContext context, string apiKey, CancellationToken cancellationToken);
}