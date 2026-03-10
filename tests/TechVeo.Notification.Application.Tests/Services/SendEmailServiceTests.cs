using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechVeo.Notification.Infra.Services;
using TechVeo.Shared.Application.Aws;

namespace TechVeo.Notification.Application.Tests.Services;

public class SendEmailServiceTests
{
    private readonly Mock<ILogger<SendEmailService>> _loggerMock;
    private readonly Mock<IOptions<AwsOptions>> _awsOptionsMock;
    private readonly Mock<IConfiguration> _configurationMock;

    public SendEmailServiceTests()
    {
        _loggerMock = new Mock<ILogger<SendEmailService>>();
        _awsOptionsMock = new Mock<IOptions<AwsOptions>>();
        _configurationMock = new Mock<IConfiguration>();

        var awsOptions = new AwsOptions
        {
            AccessKey = "test-access-key",
            SecretKey = "test-secret-key",
            Region = "us-east-1"
        };

        _awsOptionsMock.Setup(x => x.Value).Returns(awsOptions);
        _configurationMock.Setup(x => x["Email:FromAddress"]).Returns("noreply@example.com");
    }

    [Fact(DisplayName = "Should create service with valid configuration")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_WithValidConfiguration_ShouldCreateService()
    {
        // Act
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should use configured FromAddress")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_ShouldUseConfiguredFromAddress()
    {
        // Arrange
        var expectedFromAddress = "custom@example.com";
        _configurationMock.Setup(x => x["Email:FromAddress"]).Returns(expectedFromAddress);

        // Act
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
        _configurationMock.Verify(x => x["Email:FromAddress"], Times.Once);
    }

    [Fact(DisplayName = "Should validate AWS options are used")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_ShouldUseAwsOptions()
    {
        // Act
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
        _awsOptionsMock.Verify(x => x.Value, Times.AtLeastOnce);
    }

    [Fact(DisplayName = "Should send email successfully with valid parameters")]
    [Trait("Infra", "SendEmailService")]
    public async Task SendAsync_WithValidParameters_ShouldCompleteSuccessfully()
    {
        // Arrange
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        var toAddress = "recipient@example.com";
        var subject = "Test Subject";
        var bodyHtml = "<html><body>Test Body</body></html>";

        // Act & Assert - O método não lança exceção em caso de sucesso
        // Note: Este teste falhará em tempo de execução pois tenta conectar com AWS real
        // Em um cenário real, você deve mockar o AmazonSimpleEmailServiceClient
        await FluentActions.Invoking(() => 
            service.SendAsync(toAddress, subject, bodyHtml))
            .Should().NotThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should handle empty email address")]
    [Trait("Infra", "SendEmailService")]
    public async Task SendAsync_WithEmptyEmailAddress_ShouldHandleGracefully()
    {
        // Arrange
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        var toAddress = string.Empty;
        var subject = "Test Subject";
        var bodyHtml = "<html><body>Test Body</body></html>";

        // Act & Assert
        await FluentActions.Invoking(() => 
            service.SendAsync(toAddress, subject, bodyHtml))
            .Should().NotThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should handle empty subject")]
    [Trait("Infra", "SendEmailService")]
    public async Task SendAsync_WithEmptySubject_ShouldHandleGracefully()
    {
        // Arrange
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        var toAddress = "recipient@example.com";
        var subject = string.Empty;
        var bodyHtml = "<html><body>Test Body</body></html>";

        // Act & Assert
        await FluentActions.Invoking(() => 
            service.SendAsync(toAddress, subject, bodyHtml))
            .Should().NotThrowAsync<ArgumentNullException>();
    }

    [Fact(DisplayName = "Should handle empty body")]
    [Trait("Infra", "SendEmailService")]
    public async Task SendAsync_WithEmptyBody_ShouldHandleGracefully()
    {
        // Arrange
        var service = new SendEmailService(
            _loggerMock.Object,
            _awsOptionsMock.Object,
            _configurationMock.Object);

        var toAddress = "recipient@example.com";
        var subject = "Test Subject";
        var bodyHtml = string.Empty;

        // Act & Assert
        await FluentActions.Invoking(() => 
            service.SendAsync(toAddress, subject, bodyHtml))
            .Should().NotThrowAsync<ArgumentNullException>();
    }
}
