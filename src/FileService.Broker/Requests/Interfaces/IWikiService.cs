using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IWikiService
  {
    Task<List<Guid>> CheckArticlesAsync(List<Guid> atriclesIds, List<string> errors = null);
  }
}
