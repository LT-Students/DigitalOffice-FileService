using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IGetFilesCommand
  {
    Task<List<(byte[] content, string extension, string name)>> ExecuteAsync(List<Guid> filesIds, FileSource fileSource);
  }
}
