using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineBanking.Modules.Transactions.Infrastructure;

public sealed class TransactionsDbContextFactory : IDesignTimeDbContextFactory<TransactionsDbContext>
{
    public TransactionsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TransactionsDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=online_banking;Username=postgres;Password=SET_LOCALLY")
            .Options;
        return new TransactionsDbContext(options);
    }
}
