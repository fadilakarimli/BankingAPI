using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineBanking.Modules.Accounts.Infrastructure;

public sealed class AccountsDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
{
    public AccountsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AccountsDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=online_banking;Username=postgres;Password=SET_LOCALLY")
            .Options;
        return new AccountsDbContext(options);
    }
}
