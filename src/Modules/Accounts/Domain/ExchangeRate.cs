namespace OnlineBanking.Modules.Accounts.Domain;

public sealed class ExchangeRate
{
    private ExchangeRate() { }

    public ExchangeRate(Currency fromCurrency, Currency toCurrency, decimal rate)
    {
        Id = Guid.NewGuid();
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Currency FromCurrency { get; private set; }
    public Currency ToCurrency { get; private set; }
    public decimal Rate { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
}
