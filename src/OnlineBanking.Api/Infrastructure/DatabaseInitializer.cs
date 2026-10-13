using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Domain;
using OnlineBanking.Modules.Accounts.Infrastructure;
using OnlineBanking.Modules.Identity.Infrastructure;
using OnlineBanking.Modules.Loans.Infrastructure;
using OnlineBanking.Modules.Notifications.Infrastructure;
using OnlineBanking.Modules.Transactions.Infrastructure;

namespace OnlineBanking.Api.Infrastructure;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        await serviceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<AccountsDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<TransactionsDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<NotificationsDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<LoansDbContext>().Database.MigrateAsync();

        var accountsDb = serviceProvider.GetRequiredService<AccountsDbContext>();
        if (!await accountsDb.ExchangeRates.AnyAsync())
        {
            accountsDb.ExchangeRates.AddRange(
                new ExchangeRate(Currency.Azn, Currency.Usd, 0.58823529m),
                new ExchangeRate(Currency.Usd, Currency.Azn, 1.7m));
            await accountsDb.SaveChangesAsync();
        }
    }
}
