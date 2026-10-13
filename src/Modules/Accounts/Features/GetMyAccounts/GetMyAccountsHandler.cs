using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Infrastructure;

namespace OnlineBanking.Modules.Accounts.Features.GetMyAccounts;

public sealed class GetMyAccountsHandler(AccountsDbContext dbContext)
    : IRequestHandler<GetMyAccountsQuery, IReadOnlyList<AccountResponse>>
{
    public async Task<IReadOnlyList<AccountResponse>> Handle(
        GetMyAccountsQuery request,
        CancellationToken cancellationToken)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => new AccountResponse(
                x.Id,
                x.AccountNumber,
                x.Type.ToString(),
                x.Currency.ToString(),
                x.Balance,
                x.IsFrozen,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
