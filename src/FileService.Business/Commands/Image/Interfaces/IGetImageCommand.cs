using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces
{
    [AutoInject]
    public interface IGetImageCommand
    {
        OperationResultResponse<ImageInfo> Execute(Guid imageId);
    }
}
