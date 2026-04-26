using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;
using Notifications.API.Persistence;

namespace Notifications.API.Service.AuthService;

public class AuthService(AppDbContext db) : IAuthService
{
    public async Task<string> CreateApiKey(string owner, string desc, string createBy)
    {
        var key = GenerateKey();

        var entity = new ApiKey
        {
            Key = key,
            Owner = owner,
            Desc = desc,
            CreateBy = createBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.ApiKeys.Add(entity);
        await db.SaveChangesAsync();

        return key;
    }

    public async Task<AuthResponse?> LoginAsync(HttpContext context, string apiKey)
    {
        var entity = await ValidateApiKey(apiKey);
        if (entity == null)
            return null;

        var claims = CreateClaims(entity);
        var principal = CreatePrincipal(claims);

        await SignInAsync(context, principal);

        return new AuthResponse
        {
            Owner = entity.Owner,
            Desc = entity.Desc,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreateBy = entity.CreateBy
        };
    }

    private async Task<ApiKey?> ValidateApiKey(string key)
    {
        return await db.ApiKeys
            .FirstOrDefaultAsync(x => x.Key == key);
    }

    private List<Claim> CreateClaims(ApiKey entity)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, entity.Id.ToString()),
            new("owner", entity.Owner),
            new("description", entity.Desc ?? ""),
            new("createdAt", entity.CreatedAt.ToString("O")),
            new("updatedAt", entity.UpdatedAt.ToString("O"))
        };
    }

    private ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }

    private async Task SignInAsync(HttpContext context, ClaimsPrincipal principal)
    {
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
    }

    private string GenerateKey(int length = 32)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var result = new StringBuilder(length);
        var buffer = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);

        for (int i = 0; i < length; i++)
        {
            var index = buffer[i] % chars.Length;
            result.Append(chars[index]);
        }

        return result.ToString();
    }
}