﻿using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpPost("add")]
        public Guid Add(
            [FromBody] ImageRequest request,
            [FromServices] IAddImageCommand command)
        {
            return command.Execute(request);
        }
    }
}
