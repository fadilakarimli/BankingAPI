using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Transactions.Features.Transfer;

public static class TransferEndpoint
{
    public static IEndpointRouteBuilder MapTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/transactions/transfer", async (
            TransferRequest request,
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
                    new TransferCommand(userId, request.SourceAccountId, request.DestinationAccountNumber, request.Amount),
                    cancellationToken);
                return Results.Ok(result);
            }
            catch (InvalidOperationException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
            catch (ArgumentOutOfRangeException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}

public sealed record TransferRequest(
    Guid SourceAccountId,
    string DestinationAccountNumber,
    decimal Amount);
