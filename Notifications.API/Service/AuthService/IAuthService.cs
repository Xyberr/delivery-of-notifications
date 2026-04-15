using Microsoft.AspNetCore.Mvc;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<string> GenerateAndSaveToken();
}