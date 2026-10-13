using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineBanking.Modules.Identity.Domain;
using OnlineBanking.Modules.Identity.Infrastructure;

namespace OnlineBanking.Modules.Identity;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<Features.Register.RegisterHandler>());

        return services;
    }
}
