using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Notifications.Features.GetNotifications;

namespace OnlineBanking.Modules.Notifications;

public static class NotificationsEndpoints
{
    public static IEndpointRouteBuilder MapNotificationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetNotificationsEndpoint();
        return endpoints;
    }
}
