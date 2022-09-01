using MassTransit;
using RabbitMQWithNET6.Common.Commands;

namespace RabbitMQWithNET6.Consumer.Concrete
{
    public class CommandMessageConsumer : IConsumer<CommandMessage>
    {
        public async Task Consume(ConsumeContext<CommandMessage> context)
        {
            var message = context.Message;
            await Console.Out.WriteLineAsync($"Message from Producer : {message.MessageString}");
            // Do something useful with the message
        }
    }
}
