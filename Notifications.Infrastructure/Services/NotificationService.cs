using Notifications.Contracts.Requests;
using Notifications.Core.Interfaces;
using Notifications.Infrastructure.Entities;
using Notifications.Infrastructure.Persistence.DbContext;

namespace Notifications.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;

    public NotificationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> CreateAsync(CreateNotificationRequest request)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message,
            Status = "Pending"
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();

        return notification.Id;
    }
}