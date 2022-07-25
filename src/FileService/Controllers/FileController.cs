using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.FileService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FileController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<List<Guid>>> CreateAsync(
      [FromServices] ICreateFileCommand command,
      [FromQuery] Guid entityId,
      [FromQuery] FileAccessType access,
      [FromForm] IFormFileCollection uploadedFiles)
    {
      return await command.ExecuteAsync(entityId, access, uploadedFiles);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<List<FileInfo>>> GetAsync(
      [FromServices] IGetFileCommand command,
      [FromQuery] List<Guid> filesIds)
    {
      return await command.ExecuteAsync(filesIds);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditFileCommand command,
      [FromQuery] Guid fileId,
      [FromBody] JsonPatchDocument<EditFileRequest> request)
    {
      return await command.ExecuteAsync(fileId, request);
    }
  }
}
