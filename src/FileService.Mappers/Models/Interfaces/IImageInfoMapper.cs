using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IImageInfoMapper
    {
        ImageInfo Map(DbImage image);
    }
}
