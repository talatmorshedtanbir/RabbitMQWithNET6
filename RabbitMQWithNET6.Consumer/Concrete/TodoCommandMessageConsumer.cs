using MassTransit;
using RabbitMQWithNET6.Common.Commands;

namespace RabbitMQWithNET6.Consumer.Concrete
{
    public class TodoCommandMessageConsumer : IConsumer<TodoCommandMessage>
    {
        public async Task Consume(ConsumeContext<TodoCommandMessage> context)
        {
            var message = context.Message;
            await Console.Out.WriteLineAsync($"Message from Producer : {message.todoModel.TaskDescription}");
            // Do something useful with the message
        }
    }
}
