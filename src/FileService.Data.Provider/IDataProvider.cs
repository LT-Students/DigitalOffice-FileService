using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Database;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data.Provider
{
    public interface IDataProvider : IBaseDataProvider
    {
        DbSet<DbFile> Files { get; set; }
    }
}
