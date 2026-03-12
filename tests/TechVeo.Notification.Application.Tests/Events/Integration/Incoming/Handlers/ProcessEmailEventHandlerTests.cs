using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TechVeo.Notification.Application.Events.Integration.Incoming;
using TechVeo.Notification.Application.Events.Integration.Incoming.Handlers;
using TechVeo.Notification.Application.Services;
using TechVeo.Notification.Application.Enum;

namespace TechVeo.Notification.Application.Tests.Events.Integration.Incoming.Handlers;

public class ProcessEmailEventHandlerTests
{
    private readonly Mock<ISendEmailService> _sendEmailServiceMock;
    private readonly Mock<ILogger<ProcessEmailEventHandler>> _loggerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly ProcessEmailEventHandler _handler;

    public ProcessEmailEventHandlerTests()
    {
        _sendEmailServiceMock = new Mock<ISendEmailService>();
        _loggerMock = new Mock<ILogger<ProcessEmailEventHandler>>();
        _configMock = new Mock<IConfiguration>();

        _handler = new ProcessEmailEventHandler(
            _sendEmailServiceMock.Object,
            _loggerMock.Object,
            _configMock.Object);
    }

    [Fact(DisplayName = "Should send success email when status is Completed")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_WithCompletedStatus_ShouldSendSuccessEmail()
    {
        // Arrange
        var emailAddress = "user@example.com";
        var fileName = "test-video.mp4";
        var url = "https://example.com/video/123";
        var emailEvent = new SendEmailEvent(emailAddress, fileName, StatusType.Completed, url);

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(
                emailAddress,
                It.IsAny<string>(),
                It.Is<string>(body => body.Contains(fileName) && body.Contains(url))),
            Times.Once);
    }

    [Fact(DisplayName = "Should send failure email when status is Failed")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_WithFailedStatus_ShouldSendFailureEmail()
    {
        // Arrange
        var emailAddress = "user@example.com";
        var fileName = "test-video.mp4";
        var url = "https://example.com/video/123";
        var emailEvent = new SendEmailEvent(emailAddress, fileName, StatusType.Failed, url);

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(
                emailAddress,
                It.IsAny<string>(),
                It.Is<string>(body => body.Contains(fileName))),
            Times.Once);
    }

    [Fact(DisplayName = "Should send email with correct subject")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_ShouldSendEmailWithCorrectSubject()
    {
        // Arrange
        var emailEvent = new SendEmailEvent(
            "user@example.com",
            "test-video.mp4",
            StatusType.Completed,
            "https://example.com/video/123");

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(
                It.IsAny<string>(),
                "Teste de Email via AWS SES e C#",
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact(DisplayName = "Should call SendAsync exactly once")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_ShouldCallSendAsyncOnce()
    {
        // Arrange
        var emailEvent = new SendEmailEvent(
            "user@example.com",
            "test-video.mp4",
            StatusType.Completed,
            "https://example.com/video/123");

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact(DisplayName = "Should handle empty file name gracefully")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_WithEmptyFileName_ShouldSendEmailSuccessfully()
    {
        // Arrange
        var emailEvent = new SendEmailEvent(
            "user@example.com",
            string.Empty,
            StatusType.Completed,
            "https://example.com/video/123");

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact(DisplayName = "Should handle empty URL gracefully")]
    [Trait("Application", "ProcessEmailEventHandler")]
    public async Task Handle_WithEmptyUrl_ShouldSendEmailSuccessfully()
    {
        // Arrange
        var emailEvent = new SendEmailEvent(
            "user@example.com",
            "test-video.mp4",
            StatusType.Completed,
            string.Empty);

        _sendEmailServiceMock
            .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(emailEvent, CancellationToken.None);

        // Assert
        _sendEmailServiceMock.Verify(
            x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }
}
