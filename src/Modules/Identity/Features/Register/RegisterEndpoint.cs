using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace OnlineBanking.Modules.Identity.Features.Register;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/auth/register", async (
            RegisterCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Created($"api/users/{result.UserId}", result);
        });

        return endpoints;
    }
}
