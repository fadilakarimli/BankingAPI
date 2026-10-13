using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Loans.Domain;

namespace OnlineBanking.Modules.Loans.Infrastructure;

public sealed class LoansDbContext(DbContextOptions<LoansDbContext> options) : DbContext(options)
{
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var loan = modelBuilder.Entity<Loan>();
        loan.ToTable("loans", "loans");
        loan.HasKey(x => x.Id);
        loan.Property(x => x.Principal).HasPrecision(18, 2);
        loan.Property(x => x.AnnualInterestRate).HasPrecision(8, 4);
        loan.Property(x => x.MonthlyPayment).HasPrecision(18, 2);
        loan.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        loan.Property(x => x.Purpose).HasMaxLength(1000).IsRequired();
        loan.Property(x => x.RejectionReason).HasMaxLength(1000);
        loan.HasIndex(x => new { x.UserId, x.Status });
    }
}
