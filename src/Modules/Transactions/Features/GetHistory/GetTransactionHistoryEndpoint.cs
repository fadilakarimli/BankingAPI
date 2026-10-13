using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Transactions.Features.GetHistory;

public static class GetTransactionHistoryEndpoint
{
    public static IEndpointRouteBuilder MapGetTransactionHistoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/transactions", async (
            DateTimeOffset? fromUtc,
            DateTimeOffset? toUtc,
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(
                new GetTransactionHistoryQuery(userId, fromUtc, toUtc),
                cancellationToken);

            return Results.Ok(result);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}
