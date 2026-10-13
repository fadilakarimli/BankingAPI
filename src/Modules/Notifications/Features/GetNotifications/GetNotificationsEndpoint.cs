using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Notifications.Features.GetNotifications;

public static class GetNotificationsEndpoint
{
    public static IEndpointRouteBuilder MapGetNotificationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/notifications", async (
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Results.Unauthorized();
            }

            return Results.Ok(await sender.Send(new GetNotificationsQuery(userId), cancellationToken));
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}
