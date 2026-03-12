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
        _configurationMock.Setup(x => x["Email:Password"]).Returns("secret-password");
    }

    [Fact(DisplayName = "Should create service with valid configuration")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_WithValidConfiguration_ShouldCreateService()
    {
        // Act
        var service = new SendEmailService(_configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should read FromAddress from configuration")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_ShouldReadFromAddressFromConfig()
    {
        // Arrange
        var expectedFromAddress = "custom@example.com";
        _configurationMock.Setup(x => x["Email:FromAddress"]).Returns(expectedFromAddress);

        // Act
        var service = new SendEmailService(_configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
        _configurationMock.Verify(x => x["Email:FromAddress"], Times.Once);
    } 

    [Fact(DisplayName = "Should read Password from configuration")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_ShouldReadPasswordFromConfig()
    {
        // Act
        var service = new SendEmailService(_configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
        _configurationMock.Verify(x => x["Email:Password"], Times.Once);
    }

    [Fact(DisplayName = "Should create service with different FromAddress values")]
    [Trait("Infra", "SendEmailService")]
    public void Constructor_WithDifferentFromAddresses_ShouldCreateService()
    {
        // Arrange
        var addresses = new[] { "a@b.com", "test@domain.org", "sender@company.net" };

        foreach (var address in addresses)
        {
            _configurationMock.Setup(x => x["Email:FromAddress"]).Returns(address);

            // Act
            var service = new SendEmailService(_configurationMock.Object);

            // Assert
            service.Should().NotBeNull();
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
