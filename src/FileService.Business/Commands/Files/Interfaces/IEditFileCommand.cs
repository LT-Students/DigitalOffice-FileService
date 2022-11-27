using System;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IEditFileCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, FileSource fileSource, string newName);
  }
}
