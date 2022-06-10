using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<List<Guid>> CreateAsync(List<DbFile> files);

    Task<List<DbFile>> GetAsync(List<Guid> filesIds);

    Task<List<Guid>> RemoveAsync(List<Guid> filesIds);

    Task<bool> EditAsync(Guid fileId, JsonPatchDocument<DbFile> request);
  }
}
