using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbFileMapper
    {
        DbFile Map(FileRequest file);
    }
}
