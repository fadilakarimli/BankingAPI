using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Identity.Domain;
using OnlineBanking.Modules.Identity.Infrastructure;

namespace OnlineBanking.Modules.Identity.Features.Login;

public sealed class LoginHandler(
    IdentityDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService tokenService) : IRequestHandler<LoginCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null || user.IsBlocked)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var tokens = tokenService.CreateTokens(user);
        var refreshToken = new RefreshToken(
            user.Id,
            tokenService.HashRefreshToken(tokens.RefreshToken),
            DateTimeOffset.UtcNow.AddDays(7));

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return tokens;
    }
}
