using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface ICreateFilesCommand
  {
    Task<OperationResultResponse<List<Guid>>> ExecuteAsync(Guid entityId, ServiceType serviceType, FileAccessType access, IFormFileCollection uploadedFile);
  }
}
