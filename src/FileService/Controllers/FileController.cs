﻿using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("add")]
        public Guid AddNewFile(
            [FromBody] FileRequest request,
            [FromServices] IAddFileCommand command)
        {
            return command.Execute(request);
        }

        [HttpGet("get")]
        public FileInfo GetFile([FromServices] IGetFileCommand command, [FromQuery] Guid fileId)
        {
            return command.Execute(fileId);
        }

        [HttpDelete("disable")]
        public void DisableFile(
            [FromServices] IDisableFileCommand command,
            [FromQuery] Guid fileId)
        {
            command.Execute(fileId);
        }
    }
}