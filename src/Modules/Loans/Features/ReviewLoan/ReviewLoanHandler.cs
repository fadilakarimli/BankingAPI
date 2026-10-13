using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Loans.Features.SubmitLoan;
using OnlineBanking.Modules.Loans.Infrastructure;

namespace OnlineBanking.Modules.Loans.Features.ReviewLoan;

public sealed class ReviewLoanHandler(LoansDbContext dbContext)
    : IRequestHandler<ReviewLoanCommand, LoanResponse>
{
    public async Task<LoanResponse> Handle(ReviewLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await dbContext.Loans.SingleOrDefaultAsync(x => x.Id == request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException("Loan was not found.");

        if (request.Approve)
        {
            loan.Approve();
        }
        else
        {
            loan.Reject(request.RejectionReason ?? string.Empty);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return SubmitLoanHandler.ToResponse(loan);
    }
}
