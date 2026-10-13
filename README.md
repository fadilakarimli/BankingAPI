# Online Banking

Online banking backend built with .NET 10, PostgreSQL, JWT, CQRS, MediatR, and a modular monolith architecture.

## Structure

- `src/OnlineBanking.Api` - HTTP API and composition root
- `src/BuildingBlocks` - shared cross-cutting abstractions
- `src/Modules` - business modules organized by bounded context
- `tests` - unit and integration tests

Modules:

- Identity
- Users
- Accounts
- Transactions
- Loans
- Notifications
- Admin

## Run

```powershell
dotnet restore OnlineBanking.slnx
dotnet run --project src/OnlineBanking.Api
```

PostgreSQL is configured separately from the application container.

For local secrets, use .NET user-secrets or environment variables. Do not commit database passwords, JWT secrets, or SMTP credentials.
