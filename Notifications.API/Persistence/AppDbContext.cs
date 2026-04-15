using Microsoft.EntityFrameworkCore;
using Notifications.API.Entities;

namespace Notifications.API.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Token> Tokens { get; set; }
}