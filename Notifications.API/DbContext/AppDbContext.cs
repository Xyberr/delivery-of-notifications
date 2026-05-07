using Microsoft.EntityFrameworkCore;
using Notifications.API.Common;
using Notifications.API.Entities;
using Notifications.API.Entities.Enums;

namespace Notifications.API.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Recipient> MessageRecipients { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var now = DateTime.UtcNow;

        var contactTypes = Enum.GetValues(typeof(ContactTypeCode))
            .Cast<ContactTypeCode>()
            .Where(code => code != ContactTypeCode.NotSupported)
            .Select(code => new ContactType
            {
                Id = (long)code,
                Code = code,
                Name = code.GetDisplayName(),
                Description = code.GetDisplayDescription() ?? string.Empty,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        modelBuilder.Entity<ContactType>().HasData(contactTypes);
    

    var statuses = Enum.GetValues(typeof(DeliveryStatusCode))
            .Cast<DeliveryStatusCode>()
            .Select(code => new DeliveryStatus
            {
                Id = (long)code,
                Code = code,
                Name = code.GetDisplayName(),
                Description = code.GetDisplayDescription() ?? string.Empty,
                CreatedAt = now,
                UpdatedAt = now
            })
            .ToList();

        modelBuilder.Entity<DeliveryStatus>().HasData(statuses);

        base.OnModelCreating(modelBuilder);
    }
}