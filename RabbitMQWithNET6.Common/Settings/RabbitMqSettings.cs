namespace RabbitMQWithNET6.Common.Settings
{
    public class RabbitMqSettings
    {
        public string Uri { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TodoQueue { get; set; } = null!;
        public string NotificationServiceQueue { get; set; } = null!;
    }
}
