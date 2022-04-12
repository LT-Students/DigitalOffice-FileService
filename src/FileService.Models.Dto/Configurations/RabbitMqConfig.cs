using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Project;

namespace LT.DigitalOffice.FileService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string CreateFilesEndpoint { get; set; }
    public string RemoveFilesEndpoint { get; set; }

    // project

    [AutoInjectRequest(typeof(ICheckProjectFilesAccessesRequest))]
    public string CheckFilesAccessesEndpoint { get; set; }
  }
}
