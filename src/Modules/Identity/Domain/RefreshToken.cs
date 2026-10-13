namespace OnlineBanking.Modules.Identity.Domain;

public sealed class RefreshToken
{
    private RefreshToken() { }

    public RefreshToken(Guid userId, string tokenHash, DateTimeOffset expiresAtUtc)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }

    public void Revoke() => RevokedAtUtc = DateTimeOffset.UtcNow;
}
