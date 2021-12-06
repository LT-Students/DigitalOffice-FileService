using System;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
  [AutoInject]
  public interface IEditFileCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid fileId, JsonPatchDocument<EditFileRequest> request);
  }
}
