using Notifications.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddAppDatabase(builder.Configuration)
    .AddAppServices()
    .AddAppAuth()
    .AddAppSwagger();

var app = builder.Build();

app.UseAppPipeline();

app.MapControllers();

app.Run();