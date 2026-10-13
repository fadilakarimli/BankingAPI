using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Accounts.Domain;

namespace OnlineBanking.Modules.Accounts.Features.CreateAccount;

public static class CreateAccountEndpoint
{
    public static IEndpointRouteBuilder MapCreateAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/accounts", async (
            CreateAccountRequest request,
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(
                new CreateAccountCommand(userId, request.Type, request.Currency),
                cancellationToken);

            return Results.Created($"api/accounts/{result.Id}", result);
        })
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Customer,Admin" });

        return endpoints;
    }
}

public sealed record CreateAccountRequest(AccountType Type, Currency Currency);
