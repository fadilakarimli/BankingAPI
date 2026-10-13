using MediatR;
using OnlineBanking.Modules.Loans.Domain;
using OnlineBanking.Modules.Loans.Infrastructure;

namespace OnlineBanking.Modules.Loans.Features.SubmitLoan;

public sealed class SubmitLoanHandler(LoansDbContext dbContext)
    : IRequestHandler<SubmitLoanCommand, LoanResponse>
{
    public async Task<LoanResponse> Handle(SubmitLoanCommand request, CancellationToken cancellationToken)
    {
        if (request.Principal <= 0 || request.TermMonths <= 0 || request.AnnualInterestRate < 0)
        {
            throw new ArgumentException("Loan amount, term, and interest rate must be valid.");
        }

        var loan = new Loan(
            request.UserId,
            request.Principal,
            request.AnnualInterestRate,
            request.TermMonths,
            request.Purpose);

        dbContext.Loans.Add(loan);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(loan);
    }

    internal static LoanResponse ToResponse(Loan loan) => new(
        loan.Id,
        loan.Principal,
        loan.AnnualInterestRate,
        loan.TermMonths,
        loan.MonthlyPayment,
        loan.Purpose,
        loan.Status.ToString(),
        loan.RejectionReason,
        loan.CreatedAtUtc);
}
