using MassTransit;

namespace RabbitMQWithNET6.Consumer.Abstract
{
    public interface IConsumer<in TMessage> : IConsumer
    where TMessage : class
    {
        Task Consume(ConsumeContext<TMessage> context);
    }
    public interface IConsumer { }
}
