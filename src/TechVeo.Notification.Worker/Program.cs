using Microsoft.AspNetCore.Builder;
using TechVeo.Notification.Infra;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddInfra();
}

var app = builder.Build();
{
    app.Run();
}
