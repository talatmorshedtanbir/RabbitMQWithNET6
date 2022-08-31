using MassTransit;
using Microsoft.OpenApi.Models;
using RabbitMQWithNET6.Common.Commands;
using RabbitMQWithNET6.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configure Rabbit MQ
var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

builder.Services.AddMassTransit(mt => mt.AddMassTransit(x =>
{
    x.UsingRabbitMq((cntxt, cfg) =>
    {
        cfg.Host(new Uri(rabbitMqSettings.Uri), "/", c =>
        {
            c.Username(rabbitMqSettings.UserName);
            c.Password(rabbitMqSettings.Password);
        });
    });
}));

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

app.Run();