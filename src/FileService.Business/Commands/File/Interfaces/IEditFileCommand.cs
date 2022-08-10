using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
  [AutoInject]
  public interface IEditFileCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, string name);
  }
}
