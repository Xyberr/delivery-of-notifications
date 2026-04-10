using Microsoft.EntityFrameworkCore;
using Notifications.Contracts.DTO.Responses;
using Notifications.Contracts.Requests;
using Notifications.Core.Entities;
using Notifications.Infrastructure.Persistence.DbContext;

namespace Notifications.Infrastructure.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserResponse>> GetAll()
    {
        return await _db.Users
            .Select(x => new UserResponse(x.Id, x.Email))
            .ToListAsync();
    }

    public async Task<UserResponse?> Get(Guid id)
    {
        return await _db.Users
            .Where(x => x.Id == id)
            .Select(x => new UserResponse(x.Id, x.Email))
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponse> Create(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = request.Password,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return new UserResponse(user.Id, user.Email);
    }

    public async Task<bool> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        user.Email = request.Email;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return true;
    }
}