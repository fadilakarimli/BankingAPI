namespace OnlineBanking.Modules.Accounts.Domain;

public enum AccountType
{
    Savings = 1,
    Current = 2
}

public enum Currency
{
    Azn = 1,
    Usd = 2
}

public sealed class Account
{
    private Account() { }

    public Account(Guid userId, AccountType type, Currency currency, string accountNumber)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Currency = currency;
        AccountNumber = accountNumber;
        Balance = 0m;
        IsFrozen = false;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public AccountType Type { get; private set; }
    public Currency Currency { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public bool IsFrozen { get; private set; }
    public decimal DailyTransferLimit { get; private set; } = 5000m;
    public decimal DailyTransferredAmount { get; private set; }
    public DateOnly DailyTransferDate { get; private set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public decimal LowBalanceThreshold { get; private set; } = 50m;
    public bool LowBalanceAlertEnabled { get; private set; } = true;
    public DateTimeOffset CreatedAtUtc { get; private set; }

    public void SetDailyTransferLimit(decimal limit)
    {
        if (limit <= 0 || limit > 5000)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }

        DailyTransferLimit = limit;
    }

    public void SetLowBalanceAlert(decimal threshold, bool enabled)
    {
        if (threshold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threshold));
        }

        LowBalanceThreshold = threshold;
        LowBalanceAlertEnabled = enabled;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0 || Balance < amount)
        {
            throw new InvalidOperationException("Insufficient account balance.");
        }

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        Balance += amount;
    }

    public void SetFrozen(bool frozen) => IsFrozen = frozen;

    public void RegisterTransfer(decimal amount)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (DailyTransferDate != today)
        {
            DailyTransferDate = today;
            DailyTransferredAmount = 0;
        }

        if (DailyTransferredAmount + amount > DailyTransferLimit)
        {
            throw new InvalidOperationException("Daily transfer limit exceeded.");
        }

        DailyTransferredAmount += amount;
    }
}
