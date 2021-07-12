using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.FileService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetFileEndpoint { get; set; }
        public string GetImageEndpoint { get; set; }
        public string GetImagesEndpoint { get; set; }
        public string AddImageEndpoint { get; set; }
    }
}
