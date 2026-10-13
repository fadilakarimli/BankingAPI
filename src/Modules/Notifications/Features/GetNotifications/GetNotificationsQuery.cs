using MediatR;

namespace OnlineBanking.Modules.Notifications.Features.GetNotifications;

public sealed record GetNotificationsQuery(Guid UserId) : IRequest<IReadOnlyList<NotificationResponse>>;

public sealed record NotificationResponse(
    Guid Id,
    string Type,
    string Subject,
    string Message,
    bool IsRead,
    DateTimeOffset CreatedAtUtc);
