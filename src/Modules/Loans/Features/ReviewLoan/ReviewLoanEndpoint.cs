using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Loans.Features.ReviewLoan;

public static class ReviewLoanEndpoint
{
    public static IEndpointRouteBuilder MapReviewLoanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPatch("api/admin/loans/{loanId:guid}/review", async (
            Guid loanId,
            ReviewLoanRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            try
            {
                return Results.Ok(await sender.Send(
                    new ReviewLoanCommand(loanId, request.Approve, request.RejectionReason),
                    cancellationToken));
            }
            catch (InvalidOperationException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        return endpoints;
    }
}

public sealed record ReviewLoanRequest(bool Approve, string? RejectionReason);
