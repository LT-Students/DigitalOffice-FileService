using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces
{
    [AutoInject]
    public interface IFindImagesCommand
    {
        OperationResultResponse<List<ImageInfo>> Execute(List<Guid> imageIds);
    }
}
