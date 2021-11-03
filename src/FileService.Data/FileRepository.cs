using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
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
  }
}
