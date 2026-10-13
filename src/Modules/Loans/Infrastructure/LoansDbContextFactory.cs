using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineBanking.Modules.Loans.Infrastructure;

public sealed class LoansDbContextFactory : IDesignTimeDbContextFactory<LoansDbContext>
{
    public LoansDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LoansDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=online_banking;Username=postgres;Password=SET_LOCALLY")
            .Options;
        return new LoansDbContext(options);
    }
}
