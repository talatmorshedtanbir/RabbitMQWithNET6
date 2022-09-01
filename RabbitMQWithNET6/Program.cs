using MassTransit;
using Microsoft.OpenApi.Models;
using RabbitMQWithNET6.Common.Commands;
using RabbitMQWithNET6.Common.Models;
using RabbitMQWithNET6.Common.Settings;
using RabbitMQWithNET6.Consumer.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Configure Rabbit MQ
var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri(rabbitMqSettings.Uri), h =>
        {
            h.Username(rabbitMqSettings.UserName);
            h.Password(rabbitMqSettings.Password);
        });

        config.ReceiveEndpoint("queue", (c) =>
        {
            c.Consumer<CommandMessageConsumer>();
        });

        config.ReceiveEndpoint(rabbitMqSettings.TodoQueue, (c) =>
        {
            c.Consumer<TodoCommandMessageConsumer>();
        });
    }));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice.Todo.Publisher", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/sendmessage", (long id, string message, IPublishEndpoint publishEndPoint) =>
{
    publishEndPoint.Publish(new CommandMessage(id, message)); ;
});

app.MapPost("/todo", async (Todo todoModel, ISendEndpointProvider sendEndpointProvider) =>
{
    if (todoModel is not null)
    {
        var endPoint = await sendEndpointProvider.GetSendEndpoint(
            new Uri(string.Concat(rabbitMqSettings.Uri, "/", rabbitMqSettings.TodoQueue)));

        await endPoint.Send(new TodoCommandMessage(todoModel));
        return Results.Ok();
    }

    return Results.BadRequest();
});

app.Run();