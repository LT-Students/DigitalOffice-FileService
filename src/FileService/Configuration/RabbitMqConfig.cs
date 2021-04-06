using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.FileService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetFileEndpoint { get; set; }
        public string AddImageEndpoint { get; set; }
    }
}
