using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IGetFilesCommand
  {
    Task<List<(byte[] content, string extension, string name)>> ExecuteAsync(List<Guid> filesIds, ServiceType serviceType);
  }
}
