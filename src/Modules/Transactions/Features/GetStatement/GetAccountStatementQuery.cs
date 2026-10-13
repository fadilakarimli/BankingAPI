using MediatR;

namespace OnlineBanking.Modules.Transactions.Features.GetStatement;

public sealed record GetAccountStatementQuery(
    Guid UserId,
    Guid AccountId,
    DateTimeOffset FromUtc,
    DateTimeOffset ToUtc) : IRequest<AccountStatementResponse>;

public sealed record AccountStatementResponse(
    Guid AccountId,
    DateTimeOffset FromUtc,
    DateTimeOffset ToUtc,
    decimal TotalDebits,
    decimal TotalCredits,
    decimal TotalFees,
    IReadOnlyList<StatementTransactionResponse> Transactions);

public sealed record StatementTransactionResponse(
    Guid Id,
    bool IsDebit,
    decimal Amount,
    decimal Fee,
    decimal ConvertedAmount,
    string Currency,
    DateTimeOffset CreatedAtUtc);
