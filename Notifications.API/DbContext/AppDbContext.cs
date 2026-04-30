using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities;

namespace Notifications.API.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageRecipient> MessageRecipients { get; set; }
    public DbSet<MessageAttachment> MessageAttachments { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ContactType>().HasData(
            new ContactType
            {
                Id = 1,
                Name = "Email",
                Code = 0,
                Description = "Email address",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ContactType
            {
                Id = 2,
                Name = "Phone",
                Code = 1,
                Description = "Phone number",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        modelBuilder.Entity<DeliveryStatus>().HasData(
            new DeliveryStatus
            {
                Id = 1,
                Name = "Pending",
                Code = 0,
                Description = "Waiting for processing",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new DeliveryStatus
            {
                Id = 2,
                Name = "Sent",
                Code = 1,
                Description = "Successfully sent",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new DeliveryStatus
            {
                Id = 3,
                Name = "Failed",
                Code = 2,
                Description = "Sending failed",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
        
        
        base.OnModelCreating(modelBuilder);
    }
}