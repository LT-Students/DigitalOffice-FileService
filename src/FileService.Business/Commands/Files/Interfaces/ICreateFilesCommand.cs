using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface ICreateFilesCommand
  {
    Task<OperationResultResponse<List<Guid>>> ExecuteAsync(Guid entityId, FileSource fileSource, FileAccessType access, IFormFileCollection uploadedFile);
  }
}
