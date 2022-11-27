using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbFile> Files { get; set; }

    Task<int> ExecuteRawSqlAsync(string query);

    IQueryable<DbFile> FromSqlRaw(string query);
  }
}
