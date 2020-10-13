using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("addNewFile")]
        public Guid AddNewFile(
            [FromBody] FileCreateRequest request,
            [FromServices] IAddNewFileCommand command)
        {
            return command.Execute(request);
        }

        [HttpGet("getFileById")]
        public File GetFileById([FromServices] IGetFileByIdCommand command, [FromQuery] Guid fileId)
        {
            return command.Execute(fileId);
        }

        [HttpDelete("disableFileById")]
        public void DeleteFileById(
            [FromServices] IDisableFileByIdCommand command,
            [FromQuery] Guid fileId)
        {
            command.Execute(fileId);
        }
    }
}