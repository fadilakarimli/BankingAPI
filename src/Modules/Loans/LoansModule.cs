using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineBanking.Modules.Loans.Features.ReviewLoan;
using OnlineBanking.Modules.Loans.Features.SubmitLoan;
using OnlineBanking.Modules.Loans.Infrastructure;

namespace OnlineBanking.Modules.Loans;

public static class LoansModule
{
    public static IServiceCollection AddLoansModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LoansDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<SubmitLoanHandler>());
        return services;
    }
}
