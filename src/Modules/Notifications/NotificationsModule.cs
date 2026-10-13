using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineBanking.Modules.Notifications.Features.GetNotifications;
using OnlineBanking.Modules.Notifications.Infrastructure;

namespace OnlineBanking.Modules.Notifications;

public static class NotificationsModule
{
    public static IServiceCollection AddNotificationsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NotificationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ILowBalanceNotifier, LowBalanceNotifier>();
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<GetNotificationsHandler>());
        return services;
    }
}
