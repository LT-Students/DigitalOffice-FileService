using LT.DigitalOffice.Kernel.Broker;

namespace LT.DigitalOffice.FileService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqOptions
    {
        public string GetFileEndpoint { get; set; }
    }
}
