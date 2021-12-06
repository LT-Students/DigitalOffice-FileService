using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data
{
  public class FileRepository : IFileRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileRepository(IDataProvider provider, IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CreateAsync(List<DbFile> files)
    {
      if (files == null || !files.Any())
      {
        return false;
      }

      _provider.Files.AddRange(files);
      await _provider.SaveAsync();

      return true;
    }

    public async Task<bool> RemoveAsync(List<Guid> filesIds)
    {
      if (filesIds == null)
      {
        return false;
      }

      IEnumerable<DbFile> files = _provider.Files
        .Where(x => filesIds.Contains(x.Id));

      _provider.Files.RemoveRange(files);

      await _provider.SaveAsync();

      return true;
    }

    public async Task<List<DbFile>> GetAsync(List<Guid> filesIds)
    {
      return await _provider.Files.Where(u => filesIds.Contains(u.Id)).ToListAsync();
    }

    public async Task<bool> EditAsync(Guid fileId, JsonPatchDocument<DbFile> request)
    {
      DbFile dbFile = await _provider.Files.FirstOrDefaultAsync(p => p.Id == fileId);

      if (dbFile == null)
      {
        return false;
      }

      request.ApplyTo(dbFile);
      dbFile.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbFile.ModifiedAtUtc = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }
  }
}
