namespace OnlineBanking.Modules.Loans.Domain;

public enum LoanStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

public sealed class Loan
{
    private Loan() { }

    public Loan(Guid userId, decimal principal, decimal annualInterestRate, int termMonths, string purpose)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Principal = principal;
        AnnualInterestRate = annualInterestRate;
        TermMonths = termMonths;
        Purpose = purpose;
        MonthlyPayment = CalculateMonthlyPayment(principal, annualInterestRate, termMonths);
        Status = LoanStatus.Pending;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Principal { get; private set; }
    public decimal AnnualInterestRate { get; private set; }
    public int TermMonths { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public LoanStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? ReviewedAtUtc { get; private set; }

    public void Approve()
    {
        EnsurePending();
        Status = LoanStatus.Approved;
        ReviewedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Reject(string reason)
    {
        EnsurePending();
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Rejection reason is required.", nameof(reason));
        }

        Status = LoanStatus.Rejected;
        RejectionReason = reason.Trim();
        ReviewedAtUtc = DateTimeOffset.UtcNow;
    }

    private void EnsurePending()
    {
        if (Status != LoanStatus.Pending)
        {
            throw new InvalidOperationException("Only pending loans can be reviewed.");
        }
    }

    private static decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int months)
    {
        var monthlyRate = annualRate / 100m / 12m;
        if (monthlyRate == 0)
        {
            return Math.Round(principal / months, 2);
        }

        var factor = (decimal)Math.Pow((double)(1 + monthlyRate), months);
        return Math.Round(principal * monthlyRate * factor / (factor - 1), 2);
    }
}
