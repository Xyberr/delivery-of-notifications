using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;

namespace Notifications.API.Service.AuthService;

public partial class AuthService
{
    private AuthResponse MapToResponse(ApiKey entity)
    {
        return new AuthResponse
        {
            Owner = entity.Owner,
            Desc = entity.Desc,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreateBy = entity.CreatedBy
        };
    }

    private List<Claim> CreateClaims(ApiKey entity)
    {
        return new()
        {
            new(ClaimTypes.NameIdentifier, entity.Id.ToString()),
            new("owner", entity.Owner),
            new("description", entity.Desc ?? ""),
            new("createdAt", entity.CreatedAt.ToString("O")),
            new("updatedAt", entity.UpdatedAt.ToString("O")),
            new("createdBy", entity.CreatedBy ?? "")
        };
    }

    private ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}