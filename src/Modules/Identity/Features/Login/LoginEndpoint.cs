using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Identity.Features.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/auth/login", async (
            LoginCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            try
            {
                return Results.Ok(await sender.Send(command, cancellationToken));
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
        });

        return endpoints;
    }
}
