using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OnlineBanking.Modules.Notifications.Infrastructure;

public interface IEmailSender
{
    Task SendAsync(string recipient, string subject, string body, CancellationToken cancellationToken);
}

public sealed class SmtpEmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendAsync(
        string recipient,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var section = configuration.GetSection("Smtp");
        using var client = new SmtpClient(section["Host"], int.Parse(section["Port"] ?? "587"))
        {
            EnableSsl = bool.Parse(section["EnableSsl"] ?? "true"),
            Credentials = new NetworkCredential(section["Username"], section["Password"])
        };

        using var message = new MailMessage(
            new MailAddress(section["FromEmail"] ?? "no-reply@example.com", section["FromName"] ?? "Online Banking"),
            new MailAddress(recipient))
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendMailAsync(message, cancellationToken);
    }
}
