using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQWithNET6.Common.Settings;
using RabbitMQWithNET6.Consumer.Concrete;

var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

// Configure Rabbit MQ
var rabbitMqSettings = config.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

Console.Title = "Notification";

var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(new Uri(rabbitMqSettings.Uri), h =>
    {
        h.Username(rabbitMqSettings.UserName);
        h.Password(rabbitMqSettings.Password);
    });
    cfg.ReceiveEndpoint(rabbitMqSettings.TodoQueue, ep =>
    {
        ep.PrefetchCount = 16;
        ep.UseMessageRetry(r => r.Interval(2, 100));
        ep.Consumer<TodoConsumerNotification>();
    });

});

bus.StartAsync();
Console.WriteLine("Listening for Todo registered events.. Press enter to exit");
Console.ReadLine();
bus.StopAsync();