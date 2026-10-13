namespace OnlineBanking.Modules.Notifications.Domain;

public enum NotificationType
{
    LowBalance = 1,
    Security = 2,
    General = 3
}

public sealed class Notification
{
    private Notification() { }

    public Notification(Guid userId, NotificationType type, string subject, string message)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Subject = subject;
        Message = message;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    public void MarkAsRead() => IsRead = true;
}
