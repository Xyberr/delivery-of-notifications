using Microsoft.AspNetCore.Mvc;
using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<ApiKey?> ValidateApiKey(string key);
    Task<string> CreateApiKey(string owner);
}