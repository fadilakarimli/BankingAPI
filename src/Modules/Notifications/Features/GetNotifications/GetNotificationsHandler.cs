using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineBanking.Modules.Notifications.Infrastructure;

namespace OnlineBanking.Modules.Notifications.Features.GetNotifications;

public sealed class GetNotificationsHandler(NotificationsDbContext dbContext)
    : IRequestHandler<GetNotificationsQuery, IReadOnlyList<NotificationResponse>>
{
    public async Task<IReadOnlyList<NotificationResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        return await dbContext.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new NotificationResponse(
                x.Id,
                x.Type.ToString(),
                x.Subject,
                x.Message,
                x.IsRead,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
