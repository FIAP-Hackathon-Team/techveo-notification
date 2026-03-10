using TechVeo.Notification.Application.Enum;
using TechVeo.Shared.Application.Events;

namespace TechVeo.Notification.Application.Events.Integration.Incoming
{
    public record SendEmailEvent(
        string EmailAddress,
        string FileName,
        StatusType Status,
        string Url
        ) : IIntegrationEvent;
}
