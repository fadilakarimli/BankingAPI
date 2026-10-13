using MediatR;
using OnlineBanking.Modules.Accounts.Domain;

namespace OnlineBanking.Modules.Accounts.Features.CreateAccount;

public sealed record CreateAccountCommand(
    Guid UserId,
    AccountType Type,
    Currency Currency) : IRequest<CreateAccountResponse>;

public sealed record CreateAccountResponse(
    Guid Id,
    string AccountNumber,
    AccountType Type,
    Currency Currency,
    decimal Balance);
