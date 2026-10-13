using MediatR;

namespace OnlineBanking.Modules.Identity.Features.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<RegisterResponse>;

public sealed record RegisterResponse(Guid UserId, string Email, string Role);
