using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<List<Guid>> CreateAsync(List<DbFile> files);

    Task<List<FileInfo>> GetFileInfoAsync(List<Guid> filesIds);

    Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(List<Guid> filesIds);

    Task<List<string>> RemoveAsync(List<Guid> filesIds);

    Task<bool> EditNameAsync(Guid fileId, string newName);
  }
}
