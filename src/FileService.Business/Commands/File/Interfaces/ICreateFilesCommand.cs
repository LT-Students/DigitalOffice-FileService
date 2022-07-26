using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
  [AutoInject]
  public interface ICreateFilesCommand
  {
    Task<OperationResultResponse<List<Guid>>> ExecuteAsync(Guid entityId, FileAccessType access, IFormFileCollection uploadedFile);
  }
}
