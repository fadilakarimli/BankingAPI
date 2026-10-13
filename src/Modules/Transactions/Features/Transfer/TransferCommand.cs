using MediatR;

namespace OnlineBanking.Modules.Transactions.Features.Transfer;

public sealed record TransferCommand(
    Guid UserId,
    Guid SourceAccountId,
    string DestinationAccountNumber,
    decimal Amount) : IRequest<TransferResponse>;

public sealed record TransferResponse(
    Guid TransactionId,
    decimal Amount,
    decimal Fee,
    decimal ConvertedAmount,
    decimal ExchangeRate,
    string Currency);
