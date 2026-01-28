using Microsoft.Extensions.DependencyInjection;
using TechVeo.Notification.Application.Services;
using TechVeo.Notification.Infra.Services;

namespace TechVeo.Notification.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddSharedInfra(new InfraOptions
        {
            ApplicationAssembly = typeof(Application.DependencyInjection).Assembly
        });

        services.AddScoped<ISendEmailService, SendEmailService>();

        return services;
    }
}
