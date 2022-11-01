using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.FileService.Broker.Publishes.Interfaces
{
  [AutoInject]
  public interface IPublish
  {
    Task CreateFilesAsync(Guid entityId, FileAccessType access, List<Guid> filesIds);

    Task CreateWikiFilesAsync(Guid entityId, List<Guid> filesIds);
  }
}
