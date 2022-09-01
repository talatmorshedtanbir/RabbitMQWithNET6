using RabbitMQWithNET6.Common.Models;

namespace RabbitMQWithNET6.Common.Commands
{
    public record TodoCommandMessage(Todo todoModel);
}
