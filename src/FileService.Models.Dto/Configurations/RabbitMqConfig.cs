using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;

namespace LT.DigitalOffice.FileService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string CreateFilesEndpoint { get; set; }
    public string RemoveFilesEndpoint { get; set; }
  }
}
