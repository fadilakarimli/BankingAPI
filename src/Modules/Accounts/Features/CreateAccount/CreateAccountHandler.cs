using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Domain;
using OnlineBanking.Modules.Accounts.Infrastructure;

namespace OnlineBanking.Modules.Accounts.Features.CreateAccount;

public sealed class CreateAccountHandler(AccountsDbContext dbContext)
    : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    public async Task<CreateAccountResponse> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var accountNumber = await GenerateAccountNumber(cancellationToken);
        var account = new Account(request.UserId, request.Type, request.Currency, accountNumber);

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateAccountResponse(
            account.Id,
            account.AccountNumber,
            account.Type,
            account.Currency,
            account.Balance);
    }

    private async Task<string> GenerateAccountNumber(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 10; attempt++)
        {
            var number = $"AZ{Random.Shared.NextInt64(1000000000, 9999999999)}";
            if (!await dbContext.Accounts.AnyAsync(x => x.AccountNumber == number, cancellationToken))
            {
                return number;
            }
        }

        throw new InvalidOperationException("Could not generate a unique account number.");
    }
}
