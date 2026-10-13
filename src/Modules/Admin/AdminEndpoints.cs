using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Accounts.Domain;

namespace OnlineBanking.Modules.Admin;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/admin/users", async (ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new ListUsersQuery(), ct)))
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        endpoints.MapPatch("api/admin/users/{userId:guid}/block", async (
            Guid userId, SetBlockedRequest request, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new SetUserBlockedCommand(userId, request.Blocked), ct);
            return Results.NoContent();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        endpoints.MapPatch("api/admin/accounts/{accountId:guid}/freeze", async (
            Guid accountId, SetFrozenRequest request, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new SetAccountFrozenCommand(accountId, request.Frozen), ct);
            return Results.NoContent();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        endpoints.MapPut("api/admin/exchange-rates", async (
            SetRateRequest request, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new SetExchangeRateCommand(request.FromCurrency, request.ToCurrency, request.Rate), ct);
            return Results.NoContent();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });

        return endpoints;
    }
}

public sealed record SetBlockedRequest(bool Blocked);
public sealed record SetFrozenRequest(bool Frozen);
public sealed record SetRateRequest(Currency FromCurrency, Currency ToCurrency, decimal Rate);
