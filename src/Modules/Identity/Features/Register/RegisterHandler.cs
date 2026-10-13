using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Identity.Domain;
using OnlineBanking.Modules.Identity.Infrastructure;

namespace OnlineBanking.Modules.Identity.Features.Register;

public sealed class RegisterHandler(
    IdentityDbContext dbContext,
    IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var exists = await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User(request.FirstName.Trim(), request.LastName.Trim(), email, string.Empty);
        var passwordHash = passwordHasher.HashPassword(user, request.Password);

        user.SetPasswordHash(passwordHash);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(user.Id, user.Email, user.Role.ToString());
    }
}
