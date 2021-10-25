using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
    [AutoInject]
    public interface IFileRepository
    {
        Guid AddFile(DbFile file);

        Task<List<DbFile>> GetAsync(List<Guid> fileId);

        void DisableFile(Guid fileId);
    }
}
