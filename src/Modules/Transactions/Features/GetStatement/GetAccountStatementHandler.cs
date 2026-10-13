using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Infrastructure;
using OnlineBanking.Modules.Transactions.Infrastructure;

namespace OnlineBanking.Modules.Transactions.Features.GetStatement;

public sealed class GetAccountStatementHandler(
    AccountsDbContext accountsDb,
    TransactionsDbContext transactionsDb)
    : IRequestHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    public async Task<AccountStatementResponse> Handle(
        GetAccountStatementQuery request,
        CancellationToken cancellationToken)
    {
        if (request.FromUtc > request.ToUtc)
        {
            throw new ArgumentException("FromUtc cannot be later than ToUtc.");
        }

        var accountExists = await accountsDb.Accounts
            .AnyAsync(x => x.Id == request.AccountId && x.UserId == request.UserId, cancellationToken);

        if (!accountExists)
        {
            throw new InvalidOperationException("Account was not found.");
        }

        var transactions = await transactionsDb.Transactions
            .AsNoTracking()
            .Where(x =>
                x.CreatedAtUtc >= request.FromUtc &&
                x.CreatedAtUtc <= request.ToUtc &&
                (x.SourceAccountId == request.AccountId || x.DestinationAccountId == request.AccountId))
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var debits = transactions
            .Where(x => x.SourceAccountId == request.AccountId)
            .Sum(x => x.Amount + x.Fee);
        var credits = transactions
            .Where(x => x.DestinationAccountId == request.AccountId)
            .Sum(x => x.ConvertedAmount);

        return new AccountStatementResponse(
            request.AccountId,
            request.FromUtc,
            request.ToUtc,
            debits,
            credits,
            transactions.Sum(x => x.Fee),
            transactions.Select(x => new StatementTransactionResponse(
                x.Id,
                x.SourceAccountId == request.AccountId,
                x.Amount,
                x.Fee,
                x.ConvertedAmount,
                x.SourceAccountId == request.AccountId
                    ? x.SourceCurrency.ToString()
                    : x.DestinationCurrency.ToString(),
                x.CreatedAtUtc)).ToList());
    }
}
