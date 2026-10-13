using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Domain;

namespace OnlineBanking.Modules.Accounts.Infrastructure;

public sealed class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var account = modelBuilder.Entity<Account>();
        account.ToTable("accounts", "accounts");
        account.HasKey(x => x.Id);
        account.Property(x => x.AccountNumber).HasMaxLength(24).IsRequired();
        account.HasIndex(x => x.AccountNumber).IsUnique();
        account.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        account.Property(x => x.Currency).HasConversion<string>().HasMaxLength(3);
        account.Property(x => x.Balance).HasPrecision(18, 2);
        account.Property(x => x.DailyTransferLimit).HasPrecision(18, 2);
        account.Property(x => x.DailyTransferredAmount).HasPrecision(18, 2);
        account.Property(x => x.LowBalanceThreshold).HasPrecision(18, 2);
        account.HasIndex(x => new { x.UserId, x.Currency });

        var exchangeRate = modelBuilder.Entity<ExchangeRate>();
        exchangeRate.ToTable("exchange_rates", "accounts");
        exchangeRate.HasKey(x => x.Id);
        exchangeRate.Property(x => x.FromCurrency).HasConversion<string>().HasMaxLength(3);
        exchangeRate.Property(x => x.ToCurrency).HasConversion<string>().HasMaxLength(3);
        exchangeRate.Property(x => x.Rate).HasPrecision(18, 8);
        exchangeRate.HasIndex(x => new { x.FromCurrency, x.ToCurrency }).IsUnique();
    }
}
