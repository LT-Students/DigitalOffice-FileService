using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.FileService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FileController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<List<Guid>>> CreateAsync(
      [FromServices] ICreateFilesCommand command,
      [FromQuery] Guid entityId,
      [FromQuery] ServiceType serviceType,
      [FromQuery] FileAccessType access,
      [FromForm] IFormFileCollection uploadedFiles)
    {
      return await command.ExecuteAsync(entityId, serviceType, access, uploadedFiles);
    }

    [HttpGet("get")]
    public async Task<List<FileContentResult>> GetAsync(
      [FromServices] IGetFilesCommand command,
      [FromQuery] List<Guid> filesIds,
      [FromQuery] ServiceType serviceType)
    {
      List<(byte[] content, string extension, string name)> result = await command.ExecuteAsync(filesIds, serviceType);

      return result.Select(file => File(file.content, file.extension, file.name)).ToList();
    }

    [HttpPut("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditFileCommand command,
      [FromQuery] Guid entityId,
      [FromQuery] Guid fileId,
      [FromQuery] ServiceType serviceType,
      [FromQuery] string newName)
    {
      return await command.ExecuteAsync(entityId, fileId, serviceType, newName);
    }
  }
}
