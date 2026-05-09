using MassTransit;
using Notifications.API.Consumers;
using Notifications.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");

var rabbitMqHost = rabbitMqSection["Host"]
                   ?? throw new InvalidOperationException("RabbitMq:Host не настроен в конфигурации");

var rabbitMqUsername = rabbitMqSection["Username"]
                       ?? throw new InvalidOperationException("RabbitMq:Username не настроен в конфигурации");

var rabbitMqPassword = rabbitMqSection["Password"]
                       ?? throw new InvalidOperationException("RabbitMq:Password не настроен в конфигурации");

var rabbitMqQueue = rabbitMqSection["Queue"]
                    ?? throw new InvalidOperationException("RabbitMq:Queue не настроен в конфигурации");

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<NotificationConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            rabbitMqHost,
            "/",
            host =>
            {
                host.Username(rabbitMqUsername);
                host.Password(rabbitMqPassword);
            });

        cfg.ReceiveEndpoint(
            rabbitMqQueue,
            endpoint =>
            {
                endpoint.ConfigureConsumer<NotificationConsumer>(context);

                endpoint.UseMessageRetry(retry =>
                {
                    retry.Interval(
                        3,
                        TimeSpan.FromSeconds(5));
                });
            });
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