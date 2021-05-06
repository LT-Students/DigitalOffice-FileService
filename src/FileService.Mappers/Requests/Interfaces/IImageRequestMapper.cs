using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Requests.Interfaces
{
    [AutoInject]
    public interface IImageRequestMapper
    {
        ImageRequest Map(IAddImageRequest imageRequest);
    }
}
