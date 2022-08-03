using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Project;

namespace LT.DigitalOffice.FileService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string RemoveFilesEndpoint { get; set; }
    public string GetFilesEndpoint { get; set; }

    // project

    [AutoInjectRequest(typeof(ICheckProjectFilesAccessesRequest))]
    public string CheckFilesAccessesEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetProjectsUsersRequest))]
    public string GetProjectsUsersEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICheckProjectAndUserExistenceRequest))]
    public string CheckProjectAndUserExistenceEndpoint { get; set; }
  }
}
