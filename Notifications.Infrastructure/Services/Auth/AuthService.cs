using Notifications.Infrastructure.Persistence.DbContext;

namespace Notifications.Infrastructure.Services.Auth;

using Microsoft.EntityFrameworkCore;
using Notifications.Core.Entities;


public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public AuthService(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<string> Register(string email, string password)
    {
        if (await _db.Users.AnyAsync(x => x.Email == email))
            throw new Exception("User already exists");

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return _jwt.GenerateToken(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        return _jwt.GenerateToken(user);
    }
}