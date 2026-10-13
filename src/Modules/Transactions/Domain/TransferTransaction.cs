using OnlineBanking.Modules.Accounts.Domain;

namespace OnlineBanking.Modules.Transactions.Domain;

public enum TransactionType
{
    Transfer = 1,
    Fee = 2
}

public sealed class TransferTransaction
{
    private TransferTransaction() { }

    public TransferTransaction(
        Guid userId,
        Guid sourceAccountId,
        Guid destinationAccountId,
        decimal amount,
        decimal fee,
        decimal convertedAmount,
        decimal exchangeRate,
        Currency sourceCurrency,
        Currency destinationCurrency)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
        Fee = fee;
        ConvertedAmount = convertedAmount;
        ExchangeRate = exchangeRate;
        SourceCurrency = sourceCurrency;
        DestinationCurrency = destinationCurrency;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid DestinationAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Fee { get; private set; }
    public decimal ConvertedAmount { get; private set; }
    public decimal ExchangeRate { get; private set; }
    public Currency SourceCurrency { get; private set; }
    public Currency DestinationCurrency { get; private set; }
    public TransactionType Type { get; private set; } = TransactionType.Transfer;
    public DateTimeOffset CreatedAtUtc { get; private set; }
}
