using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbFileMapper
    {
        DbFile Map(FileData file, Guid id);
    }
}
