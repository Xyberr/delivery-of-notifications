using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<string> CreateApiKey(string owner);
    Task<ClaimsPrincipal?> Authenticate(string apiKey);
}