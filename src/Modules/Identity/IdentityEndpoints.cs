using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Identity.Features.Login;
using OnlineBanking.Modules.Identity.Features.Register;

namespace OnlineBanking.Modules.Identity;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRegisterEndpoint();
        endpoints.MapLoginEndpoint();
        return endpoints;
    }
}
