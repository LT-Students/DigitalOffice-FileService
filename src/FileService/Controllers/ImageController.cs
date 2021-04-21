using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
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
        public Guid AddNewImage(
            [FromBody] ImageRequest request,
            [FromServices] IAddNewImageCommand command)
        {
            return command.Execute(request);
        }
    }
}
