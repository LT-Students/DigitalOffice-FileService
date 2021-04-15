using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("addNewFile")]
        public Guid AddNewFile(
            [FromBody] File request,
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
        public void DisableFileById(
            [FromServices] IDisableFileByIdCommand command,
            [FromQuery] Guid fileId)
        {
            command.Execute(fileId);
        }
    }
}