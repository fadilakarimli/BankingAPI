using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineBanking.Modules.Transactions.Features.Transfer;
using OnlineBanking.Modules.Transactions.Infrastructure;

namespace OnlineBanking.Modules.Transactions;

public static class TransactionsModule
{
    public static IServiceCollection AddTransactionsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TransactionsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<TransferHandler>());
        return services;
    }
}
