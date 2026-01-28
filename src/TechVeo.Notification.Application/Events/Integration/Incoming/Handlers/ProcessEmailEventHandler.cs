using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TechVeo.Notification.Application.Resources;
using TechVeo.Notification.Application.Services;

namespace TechVeo.Notification.Application.Events.Integration.Incoming.Handlers;

internal class ProcessEmailEventHandler(
    ISendEmailService videoProcessingService,
    ILogger<ProcessEmailEventHandler> logger) : INotificationHandler<EmailEvent>
{
    public async Task Handle(EmailEvent @event, CancellationToken cancellationToken)
    {
        var subject = "Teste de Email via AWS SES e C#";

        var bodyHtml = Templates.Success
            .Replace("{{video_name}}", @event.FileName)
            .Replace("{{download_link}}", @event.S3Url);

        await videoProcessingService.SendEmailAsync(@event.EmailAddress, subject, bodyHtml);
    }
}
