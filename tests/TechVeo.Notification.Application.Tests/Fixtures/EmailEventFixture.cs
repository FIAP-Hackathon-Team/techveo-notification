using TechVeo.Notification.Application.Events.Integration.Incoming;
using TechVeo.Notification.Application.Enum;

namespace TechVeo.Notification.Application.Tests.Fixtures;

public class EmailEventFixture
{
    public EmailEvent CreateCompletedEmailEvent(
        string emailAddress = "user@example.com",
        string fileName = "test-video.mp4",
        string url = "https://example.com/video/123")
    {
        return new EmailEvent(emailAddress, fileName, StatusType.Completed, url);
    }

    public EmailEvent CreateFailedEmailEvent(
        string emailAddress = "user@example.com",
        string fileName = "test-video.mp4",
        string url = "https://example.com/video/123")
    {
        return new EmailEvent(emailAddress, fileName, StatusType.Failed, url);
    }

    public EmailEvent CreateEmailEventWithCustomData(
        string emailAddress,
        string fileName,
        StatusType status,
        string url)
    {
        return new EmailEvent(emailAddress, fileName, status, url);
    }

    public List<EmailEvent> CreateMultipleEmailEvents(int count)
    {
        var events = new List<EmailEvent>();
        for (int i = 0; i < count; i++)
        {
            events.Add(CreateCompletedEmailEvent(
                $"user{i}@example.com",
                $"video-{i}.mp4",
                $"https://example.com/video/{i}"));
        }
        return events;
    }
}
