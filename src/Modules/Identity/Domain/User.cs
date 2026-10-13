namespace OnlineBanking.Modules.Identity.Domain;

public enum UserRole
{
    Customer = 1,
    Admin = 2
}

public sealed class User
{
    private User() { }

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRole.Customer;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    public void SetPasswordHash(string passwordHash) => PasswordHash = passwordHash;
}
