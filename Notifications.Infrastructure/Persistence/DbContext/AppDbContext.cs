using Microsoft.EntityFrameworkCore;
using Notifications.Core.Entities;
using Notifications.Infrastructure.Entities;

namespace Notifications.Infrastructure.Persistence.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<User> Users => Set<User>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}