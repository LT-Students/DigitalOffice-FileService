using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IFileDataMapper
    {
        FileData Map(DbFile dbFile);
    }
}
