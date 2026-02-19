using Microsoft.AspNetCore.Builder;
using TechVeo.Notification.Application;
using TechVeo.Notification.Infra;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddWorker();
    builder.Services.AddApplication();
    builder.Services.AddInfra();
}

var app = builder.Build();
{
    app.Run();
}
