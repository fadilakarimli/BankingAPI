using MediatR;

namespace OnlineBanking.Modules.Transactions.Features.GetHistory;

public sealed record GetTransactionHistoryQuery(
    Guid UserId,
    DateTimeOffset? FromUtc,
    DateTimeOffset? ToUtc) : IRequest<IReadOnlyList<TransactionHistoryResponse>>;

public sealed record TransactionHistoryResponse(
    Guid Id,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    decimal Fee,
    decimal ConvertedAmount,
    decimal ExchangeRate,
    string SourceCurrency,
    string DestinationCurrency,
    DateTimeOffset CreatedAtUtc);
