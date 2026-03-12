using Microsoft.Extensions.DependencyInjection;
using TechVeo.Notification.Application.Services;
using TechVeo.Notification.Infra;
using TechVeo.Notification.Infra.Services;

namespace TechVeo.Notification.Application.Tests.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact(DisplayName = "AddInfra should register ISendEmailService as SendEmailService with Scoped lifetime")]
    [Trait("Infra", "DependencyInjection")]
    public void AddInfra_ShouldRegisterISendEmailServiceAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfra();

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ISendEmailService));
        descriptor.Should().NotBeNull();
        descriptor!.ImplementationType.Should().Be(typeof(SendEmailService));
        descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact(DisplayName = "AddInfra should return the same IServiceCollection instance")]
    [Trait("Infra", "DependencyInjection")]
    public void AddInfra_ShouldReturnSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddInfra();

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact(DisplayName = "AddInfra should register only one ISendEmailService implementation")]
    [Trait("Infra", "DependencyInjection")]
    public void AddInfra_ShouldRegisterOnlyOneISendEmailServiceImplementation()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfra();

        // Assert
        var descriptors = services.Where(d => d.ServiceType == typeof(ISendEmailService)).ToList();
        descriptors.Should().HaveCount(1);
    }
}
