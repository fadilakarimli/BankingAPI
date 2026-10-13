using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Transactions.Features.GetStatement;

public static class GetAccountStatementEndpoint
{
    public static IEndpointRouteBuilder MapGetAccountStatementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/accounts/{accountId:guid}/statement", async (
            Guid accountId,
            DateTimeOffset fromUtc,
            DateTimeOffset toUtc,
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Results.Unauthorized();
            }

            try
            {
                var result = await sender.Send(
                    new GetAccountStatementQuery(userId, accountId, fromUtc, toUtc),
                    cancellationToken);
                return Results.Ok(result);
            }
            catch (InvalidOperationException exception)
            {
                return Results.NotFound(new { message = exception.Message });
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}
