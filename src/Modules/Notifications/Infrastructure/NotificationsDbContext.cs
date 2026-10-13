using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Notifications.Domain;

namespace OnlineBanking.Modules.Notifications.Infrastructure;

public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var notification = modelBuilder.Entity<Notification>();
        notification.ToTable("notifications", "notifications");
        notification.HasKey(x => x.Id);
        notification.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        notification.Property(x => x.Subject).HasMaxLength(200).IsRequired();
        notification.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        notification.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedAtUtc });
    }
}
