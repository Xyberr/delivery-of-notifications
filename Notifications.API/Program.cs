using MassTransit;
using Microsoft.Extensions.Options;
using Notifications.API.Consumers;
using Notifications.API.Contracts.Notifications;
using Notifications.API.Entities;
using Notifications.API.Extensions;
using Notifications.API.Services.DeliveryStatusProvider;
using Notifications.API.Services.Notifications;
using Notifications.API.Services.Notifications.BackgroundJobs;
using Quartz;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddOptions<RabbitMqConfig>()
    .Bind(builder.Configuration.GetSection(RabbitMqConfig.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<INotificationSender, FakeNotificationSender>();

builder.Services.AddScoped<
    IDeliveryStatusProvider,
    DeliveryStatusProvider>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("NotificationRetryJob");

    q.AddJob<NotificationRetryJob>(opts =>
        opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("NotificationRetryTrigger")
        .WithSimpleSchedule(x =>
            x.WithInterval(TimeSpan.FromSeconds(30))
                .RepeatForever()));
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

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