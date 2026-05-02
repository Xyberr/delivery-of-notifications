using Notifications.API.DTO.Responses;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<string> CreateApiKey(string owner, string desc, string createBy);
    Task<AuthResponse?> LoginAsync(HttpContext context, string apiKey);
}