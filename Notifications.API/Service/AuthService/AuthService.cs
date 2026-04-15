using System.Security.Cryptography;
using System.Text;
using Notifications.API.Entities;
using Notifications.API.Persistence;

namespace Notifications.API.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> GenerateAndSaveToken()
    {
        var tokenValue = GenerateToken();

        var token = new Token
        {
            Value = tokenValue,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tokens.Add(token);
        await _db.SaveChangesAsync();

        return tokenValue;
    }

    private string GenerateToken(int length = 32)
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