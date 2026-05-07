using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Notifications.API.DTO.Responses;

namespace Notifications.API.Service.AuthService;

public partial class AuthService
{
    public async Task<AuthResponse?> LoginAsync(
        HttpContext context,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var entity = await GetApiKeyAsync(apiKey, cancellationToken);

        if (entity == null)
            return null;

        var claims = CreateClaims(entity);
        var principal = CreatePrincipal(claims);

        await SignInAsync(context, principal);

        return MapToResponse(entity);
    }

    private async Task SignInAsync(HttpContext context, ClaimsPrincipal principal)
    {
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            principal);
    }
}