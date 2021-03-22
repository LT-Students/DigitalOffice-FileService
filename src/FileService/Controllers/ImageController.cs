using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpPost("addNewFile")]
        public Guid AddNewFile(
            [FromBody] ImageRequest request,
            [FromServices] IAddNewImageCommand command)
        {
            return command.Execute(request);
        }
    }
}
