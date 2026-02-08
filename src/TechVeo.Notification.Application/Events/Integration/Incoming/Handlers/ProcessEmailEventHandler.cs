using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TechVeo.Notification.Application.Resources;
using TechVeo.Notification.Application.Services;

namespace TechVeo.Notification.Application.Events.Integration.Incoming.Handlers;

public class ProcessEmailEventHandler : INotificationHandler<EmailEvent>
{
    private readonly ILogger _logger;
    private readonly ISendEmailService _sendEmailService;

    public ProcessEmailEventHandler(ISendEmailService sendEmailService,
        ILogger<ProcessEmailEventHandler> logger)
    {
        _sendEmailService = sendEmailService;
        _logger = logger;
    }

    public async Task Handle(EmailEvent @event, CancellationToken cancellationToken)
    {
        var subject = "Teste de Email via AWS SES e C#";

        var bodyHtml = @event.Status == Enum.StatusType.Completed
    ? Templates.Success
        .Replace("{{video_name}}", @event.FileName)
        .Replace("{{download_link}}", @event.Url)
    : Templates.Failed
        .Replace("{{video_name}}", @event.FileName)
        .Replace("{{retry_link}}", "https://seusite.com/upload");

        await _sendEmailService.SendAsync(@event.EmailAddress, subject, bodyHtml);
    }
}
