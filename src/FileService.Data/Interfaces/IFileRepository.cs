using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<bool> CreateAsync(List<DbFile> files);

    Task<List<DbFile>> GetAsync(List<Guid> filesIds);

    Task<bool> RemoveAsync(List<Guid> filesIds);
  }
}
