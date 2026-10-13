using MediatR;

namespace OnlineBanking.Modules.Accounts.Features.GetMyAccounts;

public sealed record GetMyAccountsQuery(Guid UserId) : IRequest<IReadOnlyList<AccountResponse>>;

public sealed record AccountResponse(
    Guid Id,
    string AccountNumber,
    string Type,
    string Currency,
    decimal Balance,
    bool IsFrozen,
    DateTimeOffset CreatedAtUtc);
