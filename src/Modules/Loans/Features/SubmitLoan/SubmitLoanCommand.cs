using MediatR;

namespace OnlineBanking.Modules.Loans.Features.SubmitLoan;

public sealed record SubmitLoanCommand(
    Guid UserId,
    decimal Principal,
    decimal AnnualInterestRate,
    int TermMonths,
    string Purpose) : IRequest<LoanResponse>;

public sealed record LoanResponse(
    Guid Id,
    decimal Principal,
    decimal AnnualInterestRate,
    int TermMonths,
    decimal MonthlyPayment,
    string Purpose,
    string Status,
    string? RejectionReason,
    DateTimeOffset CreatedAtUtc);
