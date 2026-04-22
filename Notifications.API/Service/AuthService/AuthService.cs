using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities;
using Notifications.API.Persistence;
using Notifications.API.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> CreateApiKey(string owner)
    {
        var key = GenerateKey();

        var entity = new ApiKey
        {
            Key = key,
            Owner = owner,
            CreatedAt = DateTime.UtcNow
        };

        _db.ApiKeys.Add(entity);
        await _db.SaveChangesAsync();

        return key;
    }

    public async Task<ClaimsPrincipal?> Authenticate(string apiKey)
    {
        var entity = await _db.ApiKeys
            .FirstOrDefaultAsync(x => x.Key == apiKey);

        if (entity == null)
            return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()),
            new Claim("owner", entity.Owner)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
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