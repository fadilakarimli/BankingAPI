using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Accounts.Features.GetMyAccounts;

public static class GetMyAccountsEndpoint
{
    public static IEndpointRouteBuilder MapGetMyAccountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/accounts", async (
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new GetMyAccountsQuery(userId), cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}
