using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.FileService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FileController : ControllerBase
  {
    [HttpGet("get")]
    public async Task<List<FileInfo>> GetAsync(
      [FromServices] IGetFileCommand command,
      [FromQuery] List<Guid> filesIds)
    {
      return await command.Execute(filesIds);
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
