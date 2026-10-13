using Microsoft.AspNetCore.Routing;
using OnlineBanking.Modules.Transactions.Features.Transfer;
using OnlineBanking.Modules.Transactions.Features.GetHistory;
using OnlineBanking.Modules.Transactions.Features.GetStatement;

namespace OnlineBanking.Modules.Transactions;

public static class TransactionsEndpoints
{
    public static IEndpointRouteBuilder MapTransactionsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapTransferEndpoint();
        endpoints.MapGetTransactionHistoryEndpoint();
        endpoints.MapGetAccountStatementEndpoint();
        return endpoints;
    }
}
