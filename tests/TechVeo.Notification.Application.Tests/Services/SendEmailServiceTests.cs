using Microsoft.Extensions.Configuration;
using TechVeo.Notification.Infra.Services;

namespace TechVeo.Notification.Application.Tests.Services;

public class SendEmailServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;

    public SendEmailServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
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
    }
}
