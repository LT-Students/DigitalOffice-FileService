using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IProjectService
  {
    Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors);
  }
}
