using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbFile> Files { get; set; }

        void Save();
        void EnsureDeleted();
        bool IsInMemory();
    }
}
