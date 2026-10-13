using MediatR;
using OnlineBanking.Modules.Identity.Infrastructure;

namespace OnlineBanking.Modules.Identity.Features.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<TokenResponse>;
