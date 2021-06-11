using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.File;

namespace LT.DigitalOffice.FileService.Mappers.Requests.Interfaces
{
    [AutoInject]
    public interface IImageRequestMapper
    {
        ImageRequest Map(IAddImageRequest imageRequest);
    }
}
