using System.Threading.Tasks;

namespace TechVeo.Notification.Application.Services;

public interface ISendEmailService
{
    Task SendAsync(string toAddress, string subject, string bodyHtml);
}
