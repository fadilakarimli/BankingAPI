using System.Net.Mail;
using OnlineBanking.Modules.Notifications.Domain;

namespace OnlineBanking.Modules.Notifications.Infrastructure;

public interface ILowBalanceNotifier
{
    Task NotifyAsync(Guid userId, string email, string accountNumber, decimal balance, string currency, CancellationToken cancellationToken);
}

public sealed class LowBalanceNotifier(
    NotificationsDbContext dbContext,
    IEmailSender emailSender) : ILowBalanceNotifier
{
    public async Task NotifyAsync(
        Guid userId,
        string email,
        string accountNumber,
        decimal balance,
        string currency,
        CancellationToken cancellationToken)
    {
        var subject = "Low balance alert";
        var message = $"Account {accountNumber} balance is {balance:0.00} {currency}.";
        dbContext.Notifications.Add(new Notification(userId, NotificationType.LowBalance, subject, message));
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            await emailSender.SendAsync(email, subject, message, cancellationToken);
        }
        catch (SmtpException)
        {
            // The database notification remains available if SMTP is temporarily unavailable.
        }
    }
}
