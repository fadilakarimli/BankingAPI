using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Loans.Features.ReviewLoan;
using OnlineBanking.Modules.Loans.Features.SubmitLoan;

namespace OnlineBanking.Modules.Loans;

public static class LoansEndpoints
{
    public static IEndpointRouteBuilder MapLoansEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapSubmitLoanEndpoint();
        endpoints.MapReviewLoanEndpoint();
        return endpoints;
    }
}
