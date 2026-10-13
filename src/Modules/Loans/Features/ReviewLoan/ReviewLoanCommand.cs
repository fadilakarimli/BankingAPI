using MediatR;
using OnlineBanking.Modules.Loans.Features.SubmitLoan;

namespace OnlineBanking.Modules.Loans.Features.ReviewLoan;

public sealed record ReviewLoanCommand(
    Guid LoanId,
    bool Approve,
    string? RejectionReason) : IRequest<LoanResponse>;
