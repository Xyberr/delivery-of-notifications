using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public interface IAuthService
{
    Task<string> CreateApiKey(string owner, string desc);
    Task<(ClaimsPrincipal? principal, AuthResponse? data)> Authenticate(string apiKey);
}