using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpPost("add")]
        public Guid Add(
            [FromBody] AddImageRequest request,
            [FromServices] IAddImageCommand command)
        {
            return command.Execute(request);
        }

        [HttpGet("get")]
        public OperationResultResponse<ImageInfo> Get(
            [FromServices] IGetImageCommand command,
            [FromQuery] Guid imageId)
        {
            return command.Execute(imageId);
        }

        [HttpGet("find")]
        public OperationResultResponse<List<ImageInfo>> Find(
            [FromServices] IFindImagesCommand command,
            [FromBody] List<Guid> imageIds)
        {
            return command.Execute(imageIds);
        }
    }
}
