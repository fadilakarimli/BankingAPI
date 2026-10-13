using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineBanking.Modules.Admin;

public static class AdminModule
{
    public static IServiceCollection AddAdminModule(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<ListUsersHandler>());
        return services;
    }
}
