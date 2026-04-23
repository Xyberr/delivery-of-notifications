using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities;

namespace Notifications.API.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<ApiKey> ApiKeys { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            
        base.OnModelCreating(modelBuilder);
    }
}