using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<List<Guid>> CreateAsync(FileSource fileSource, List<DbFile> files);

    Task<List<FileInfo>> GetFileInfoAsync(FileSource fileSource, List<Guid> filesIds);

    Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(FileSource fileSource, List<Guid> filesIds);

    Task<List<string>> RemoveAsync(FileSource fileSource, List<Guid> filesIds);

    Task<bool> EditNameAsync(FileSource fileSource, Guid fileId, string newName);
  }
}
