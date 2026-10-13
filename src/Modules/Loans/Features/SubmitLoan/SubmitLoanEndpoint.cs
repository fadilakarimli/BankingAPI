using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Loans.Features.SubmitLoan;

public static class SubmitLoanEndpoint
{
    public static IEndpointRouteBuilder MapSubmitLoanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/loans", async (
            SubmitLoanRequest request,
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
                var result = await sender.Send(new SubmitLoanCommand(
                    userId,
                    request.Principal,
                    request.AnnualInterestRate,
                    request.TermMonths,
                    request.Purpose), cancellationToken);
                return Results.Created($"api/loans/{result.Id}", result);
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

public sealed record SubmitLoanRequest(
    decimal Principal,
    decimal AnnualInterestRate,
    int TermMonths,
    string Purpose);
