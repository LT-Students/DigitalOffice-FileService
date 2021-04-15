using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces
{
    public interface IImageRequestMapper
    {
        ImageRequest Map(IAddImageRequest imageRequest);
    }
}
