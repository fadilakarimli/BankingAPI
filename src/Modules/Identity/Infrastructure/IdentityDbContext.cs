using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Identity.Domain;

namespace OnlineBanking.Modules.Identity.Infrastructure;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.ToTable("users", "identity");
        user.HasKey(x => x.Id);
        user.Property(x => x.Email).HasMaxLength(320).IsRequired();
        user.HasIndex(x => x.Email).IsUnique();
        user.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        user.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        user.Property(x => x.PasswordHash).IsRequired();
        user.Property(x => x.Role).HasConversion<string>().HasMaxLength(30);

        var refreshToken = modelBuilder.Entity<RefreshToken>();
        refreshToken.ToTable("refresh_tokens", "identity");
        refreshToken.HasKey(x => x.Id);
        refreshToken.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
        refreshToken.HasIndex(x => x.TokenHash).IsUnique();
        refreshToken.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
    }
}
