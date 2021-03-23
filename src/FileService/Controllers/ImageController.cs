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
        [HttpPost("addNewImage")]
        public Guid AddNewImage(
            [FromBody] ImageRequest request,
            [FromHeader] Guid userId,
            [FromServices] IAddNewImageCommand command)
        {
            request.UserId = userId;

            return command.Execute(request);
        }
    }
}
