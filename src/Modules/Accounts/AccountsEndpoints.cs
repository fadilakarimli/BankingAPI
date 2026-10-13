using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Accounts.Features.CreateAccount;
using OnlineBanking.Modules.Accounts.Features.GetMyAccounts;

namespace OnlineBanking.Modules.Accounts;

public static class AccountsEndpoints
{
    public static IEndpointRouteBuilder MapAccountsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateAccountEndpoint();
        endpoints.MapGetMyAccountsEndpoint();
        return endpoints;
    }
}
