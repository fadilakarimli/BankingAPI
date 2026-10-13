using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Accounts.Domain;
using OnlineBanking.Modules.Accounts.Infrastructure;
using OnlineBanking.Modules.Identity.Infrastructure;

namespace OnlineBanking.Modules.Admin;

public sealed record AdminUserResponse(Guid Id, string FirstName, string LastName, string Email, string Role, bool IsBlocked);
public sealed record ListUsersQuery : IRequest<IReadOnlyList<AdminUserResponse>>;
public sealed record SetUserBlockedCommand(Guid UserId, bool Blocked) : IRequest;
public sealed record SetAccountFrozenCommand(Guid AccountId, bool Frozen) : IRequest;
public sealed record SetExchangeRateCommand(Currency FromCurrency, Currency ToCurrency, decimal Rate) : IRequest;

public sealed class ListUsersHandler(IdentityDbContext db) : IRequestHandler<ListUsersQuery, IReadOnlyList<AdminUserResponse>>
{
    public async Task<IReadOnlyList<AdminUserResponse>> Handle(ListUsersQuery request, CancellationToken cancellationToken) =>
        await db.Users.AsNoTracking().OrderBy(x => x.CreatedAtUtc)
            .Select(x => new AdminUserResponse(x.Id, x.FirstName, x.LastName, x.Email, x.Role.ToString(), x.IsBlocked))
            .ToListAsync(cancellationToken);
}

public sealed class SetUserBlockedHandler(IdentityDbContext db) : IRequestHandler<SetUserBlockedCommand>
{
    public async Task Handle(SetUserBlockedCommand request, CancellationToken cancellationToken)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("User was not found.");
        user.SetBlocked(request.Blocked);
        await db.SaveChangesAsync(cancellationToken);
    }
}

public sealed class SetAccountFrozenHandler(AccountsDbContext db) : IRequestHandler<SetAccountFrozenCommand>
{
    public async Task Handle(SetAccountFrozenCommand request, CancellationToken cancellationToken)
    {
        var account = await db.Accounts.SingleOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException("Account was not found.");
        account.SetFrozen(request.Frozen);
        await db.SaveChangesAsync(cancellationToken);
    }
}

public sealed class SetExchangeRateHandler(AccountsDbContext db) : IRequestHandler<SetExchangeRateCommand>
{
    public async Task Handle(SetExchangeRateCommand request, CancellationToken cancellationToken)
    {
        if (request.FromCurrency == request.ToCurrency || request.Rate <= 0)
        {
            throw new ArgumentException("Currencies must differ and rate must be positive.");
        }

        var rate = await db.ExchangeRates.SingleOrDefaultAsync(
            x => x.FromCurrency == request.FromCurrency && x.ToCurrency == request.ToCurrency,
            cancellationToken);

        if (rate is null)
        {
            db.ExchangeRates.Add(new ExchangeRate(request.FromCurrency, request.ToCurrency, request.Rate));
        }
        else
        {
            db.ExchangeRates.Remove(rate);
            db.ExchangeRates.Add(new ExchangeRate(request.FromCurrency, request.ToCurrency, request.Rate));
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
