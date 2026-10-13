using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Transactions.Infrastructure;

namespace OnlineBanking.Modules.Transactions.Features.GetHistory;

public sealed class GetTransactionHistoryHandler(TransactionsDbContext dbContext)
    : IRequestHandler<GetTransactionHistoryQuery, IReadOnlyList<TransactionHistoryResponse>>
{
    public async Task<IReadOnlyList<TransactionHistoryResponse>> Handle(
        GetTransactionHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId);

        if (request.FromUtc.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc >= request.FromUtc.Value);
        }

        if (request.ToUtc.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc <= request.ToUtc.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new TransactionHistoryResponse(
                x.Id,
                x.SourceAccountId,
                x.DestinationAccountId,
                x.Amount,
                x.Fee,
                x.ConvertedAmount,
                x.ExchangeRate,
                x.SourceCurrency.ToString(),
                x.DestinationCurrency.ToString(),
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
