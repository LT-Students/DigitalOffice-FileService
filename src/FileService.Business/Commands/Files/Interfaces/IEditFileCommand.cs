using System;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IEditFileCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, ServiceType serviceType, string newName);
  }
}
