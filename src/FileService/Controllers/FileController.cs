using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("add")]
        public Guid Add(
            [FromBody] AddFileRequest request,
            [FromServices] IAddFileCommand command)
        {
            return command.Execute(request);
        }

        [HttpGet("get")]
        public async Task<List<FileInfo>> Get(
            [FromServices] IGetFileCommand command,
            [FromQuery] List<Guid> filesIds)
        {
            return await command.Execute(filesIds);
        }

        [HttpDelete("disable")]
        public void Disable(
            [FromServices] IDisableFileCommand command,
            [FromQuery] Guid fileId)
        {
            command.Execute(fileId);
        }
    }
}