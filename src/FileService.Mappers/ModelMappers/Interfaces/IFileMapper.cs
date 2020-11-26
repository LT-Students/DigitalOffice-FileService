using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;

namespace LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces
{
    public interface IFileMapper : IMapper<DbFile, File>, IMapper<File, DbFile>
    {
    }
}
