using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<List<Guid>> CreateAsync(List<DbFile> files);

    Task<List<DbFile>> GetAsync(List<Guid> filesIds);

    Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(List<Guid> filesIds);

    Task<List<Guid>> RemoveAsync(List<Guid> filesIds);

    Task<bool> EditNameAsync(Guid fileId, string name);
  }
}
