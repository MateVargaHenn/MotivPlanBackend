using MassTransit;
using MotivPlanBackend.Worker;
using MotivPlanBackend.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendConfirmationEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", h =>
        {
            h.Username("motivplan");
            h.Password("motivplan");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
await host.RunAsync();
