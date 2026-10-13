using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Infrastructure;
using OnlineBanking.Modules.Identity.Infrastructure;
using OnlineBanking.Modules.Notifications.Infrastructure;
using OnlineBanking.Modules.Transactions.Domain;
using OnlineBanking.Modules.Transactions.Infrastructure;

namespace OnlineBanking.Modules.Transactions.Features.Transfer;

public sealed class TransferHandler(
    AccountsDbContext accountsDb,
    TransactionsDbContext transactionsDb,
    IdentityDbContext identityDb,
    ILowBalanceNotifier lowBalanceNotifier) : IRequestHandler<TransferCommand, TransferResponse>
{
    public async Task<TransferResponse> Handle(
        TransferCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Amount < 0.90m || request.Amount > 5000m)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Amount), "Transfer must be between 0.90 and 5000.");
        }

        var source = await accountsDb.Accounts
            .SingleOrDefaultAsync(x => x.Id == request.SourceAccountId && x.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Source account was not found.");

        var destination = await accountsDb.Accounts
            .SingleOrDefaultAsync(x => x.AccountNumber == request.DestinationAccountNumber, cancellationToken)
            ?? throw new InvalidOperationException("Destination account was not found.");

        if (source.IsFrozen || destination.IsFrozen)
        {
            throw new InvalidOperationException("Frozen accounts cannot be used for transfers.");
        }

        var fee = source.UserId == destination.UserId
            ? 0m
            : Math.Round(request.Amount / 100m, 2, MidpointRounding.AwayFromZero);

        var exchangeRate = 1m;
        if (source.Currency != destination.Currency)
        {
            exchangeRate = await accountsDb.ExchangeRates
                .Where(x => x.FromCurrency == source.Currency && x.ToCurrency == destination.Currency)
                .Select(x => x.Rate)
                .SingleOrDefaultAsync(cancellationToken);

            if (exchangeRate <= 0)
            {
                throw new InvalidOperationException("Exchange rate was not configured.");
            }
        }

        var convertedAmount = Math.Round(request.Amount * exchangeRate, 2, MidpointRounding.AwayFromZero);
        source.RegisterTransfer(request.Amount + fee);
        source.Debit(request.Amount + fee);
        destination.Credit(convertedAmount);

        var transaction = new TransferTransaction(
            request.UserId,
            source.Id,
            destination.Id,
            request.Amount,
            fee,
            convertedAmount,
            exchangeRate,
            source.Currency,
            destination.Currency);

        accountsDb.Accounts.Update(source);
        accountsDb.Accounts.Update(destination);
        transactionsDb.Transactions.Add(transaction);

        await accountsDb.SaveChangesAsync(cancellationToken);
        await transactionsDb.SaveChangesAsync(cancellationToken);

        if (source.LowBalanceAlertEnabled && source.Balance <= source.LowBalanceThreshold)
        {
            var email = await identityDb.Users
                .Where(x => x.Id == request.UserId)
                .Select(x => x.Email)
                .SingleOrDefaultAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(email))
            {
                await lowBalanceNotifier.NotifyAsync(
                    request.UserId,
                    email,
                    source.AccountNumber,
                    source.Balance,
                    source.Currency.ToString(),
                    cancellationToken);
            }
        }

        return new TransferResponse(
            transaction.Id,
            transaction.Amount,
            transaction.Fee,
            transaction.ConvertedAmount,
            transaction.ExchangeRate,
            transaction.SourceCurrency.ToString());
    }
}
