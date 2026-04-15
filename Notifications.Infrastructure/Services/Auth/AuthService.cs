using Microsoft.EntityFrameworkCore;
using Notifications.Core.Entities;
using Notifications.Infrastructure.Persistence.DbContext;

namespace Notifications.Infrastructure.Services.Auth;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> Register(string email, string password)
    {
        if (await _db.Users.AnyAsync(x => x.Email == email))
            throw new Exception("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Validate(string email, string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}