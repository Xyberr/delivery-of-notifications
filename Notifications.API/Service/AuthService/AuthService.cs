using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Notifications.API.DTO.Responses;
using Notifications.API.Entities;
using Notifications.API.Persistence;
using Notifications.API.Service.AuthService;

public class AuthService(AppDbContext db) : IAuthService
{

    public async Task<string> CreateApiKey(string owner, string desc)
    {
        var key = GenerateKey();

        var entity = new ApiKey
        {
            Key = key,
            Owner = owner,
            Desc = desc,
            CreatedAt = DateTime.UtcNow,
        };

        db.ApiKeys.Add(entity);
        await db.SaveChangesAsync();

        return key;
    }

    public async Task<(ClaimsPrincipal?, AuthResponse?)> Authenticate(string apiKey)
    {
        var entity = await db.ApiKeys
            .FirstOrDefaultAsync(x => x.Key == apiKey);

        if (entity == null)
            return (null, null);

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, entity.Id.ToString()),
            new ("owner", entity.Owner),
            new ("description", entity.Desc ?? ""),
            new ("createdAt", entity.CreatedAt.ToString("O"))
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var data = new AuthResponse
        {
            Owner = entity.Owner,
            Desc = entity.Desc,
            CreatedAt = entity.CreatedAt
        };

        return (principal, data);
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