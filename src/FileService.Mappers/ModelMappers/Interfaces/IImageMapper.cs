using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;

namespace LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces
{
    public interface IImageMapper : IMapper<ImageRequest, DbImage>
    {
    }
}
