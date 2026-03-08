using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TechVeo.Notification.Application.Services;

namespace TechVeo.Notification.Infra.Services;

public class SendEmailService : ISendEmailService
{
    private readonly string _recipientEmail;
    private readonly string _password;
    public SendEmailService(IConfiguration config) {
        _recipientEmail = config["Email:FromAddress"]!;
        _password= config["Email:Password"]!;
    }

    public async Task SendAsync(string toAddress, string subject, string bodyHtml)
    {
        using var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_recipientEmail, _password),
            EnableSsl = true
        };

        var email = new MailMessage(_recipientEmail, toAddress, subject, bodyHtml) { IsBodyHtml = true };

        await client.SendMailAsync(email);
    }
}
