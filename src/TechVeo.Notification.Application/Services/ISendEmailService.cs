using System.Threading.Tasks;

namespace TechVeo.Notification.Application.Services;

public interface ISendEmailService
{
    Task SendEmailAsync(string toAddress, string subject, string bodyHtml);
}
