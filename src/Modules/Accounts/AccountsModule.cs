using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineBanking.Modules.Accounts.Features.CreateAccount;
using OnlineBanking.Modules.Accounts.Infrastructure;

namespace OnlineBanking.Modules.Accounts;

public static class AccountsModule
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AccountsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateAccountHandler>());
        return services;
    }
}
