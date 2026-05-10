using MassTransit;
using Microsoft.Extensions.Options;
using Notifications.API.Consumers;
using Notifications.API.Entities;
using Notifications.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddOptions<RabbitMqConfig>()
    .Bind(builder.Configuration.GetSection(RabbitMqConfig.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<NotificationConsumer>();

    configurator.SetKebabCaseEndpointNameFormatter();

    configurator.UsingRabbitMq((context, cfg) =>
    {
        var settings = context
            .GetRequiredService<IOptions<RabbitMqConfig>>()
            .Value;

        cfg.Host(
            settings.Host,
            host =>
            {
                host.Username(settings.Username);
                host.Password(settings.Password);
            });

        cfg.UseMessageRetry(retry =>
        {
            retry.Interval(
                3,
                TimeSpan.FromSeconds(5));
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services
    .AddAppDatabase(builder.Configuration)
    .AddAppServices()
    .AddAppAuth()
    .AddAppSwagger();

var app = builder.Build();

app.UseAppPipeline();

app.MapControllers();

app.Run();