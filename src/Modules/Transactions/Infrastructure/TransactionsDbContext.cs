using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Transactions.Domain;

namespace OnlineBanking.Modules.Transactions.Infrastructure;

public sealed class TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : DbContext(options)
{
    public DbSet<TransferTransaction> Transactions => Set<TransferTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var transaction = modelBuilder.Entity<TransferTransaction>();
        transaction.ToTable("transactions", "transactions");
        transaction.HasKey(x => x.Id);
        transaction.Property(x => x.Amount).HasPrecision(18, 2);
        transaction.Property(x => x.Fee).HasPrecision(18, 2);
        transaction.Property(x => x.ConvertedAmount).HasPrecision(18, 2);
        transaction.Property(x => x.ExchangeRate).HasPrecision(18, 8);
        transaction.Property(x => x.SourceCurrency).HasConversion<string>().HasMaxLength(3);
        transaction.Property(x => x.DestinationCurrency).HasConversion<string>().HasMaxLength(3);
        transaction.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        transaction.HasIndex(x => new { x.UserId, x.CreatedAtUtc });
    }
}
