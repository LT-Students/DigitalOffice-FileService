using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data
{
  public class FileRepository : IFileRepository
  {
    private readonly IDataProvider _provider;
    private readonly IFileCharacteristicsDataMapper _fileCharacteristicsDataMapper;
    private readonly IFileInfoMapper _fileInfoMapper;
    private readonly FileServiceDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileRepository(
      IDataProvider provider,
      IFileCharacteristicsDataMapper fileCharacteristicsDataMapper,
      IFileInfoMapper fileInfoMapper,
      FileServiceDbContext context,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _fileCharacteristicsDataMapper = fileCharacteristicsDataMapper;
      _fileInfoMapper = fileInfoMapper;
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Guid>> CreateAsync(List<DbFile> files)
    {
      if (files is not null && files.Any())
      {
        _provider.Files.AddRange(files);
        await _provider.SaveAsync();
      }

      return files.Select(x => x.Id).ToList();
    }

    public async Task<List<string>> RemoveAsync(List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      List<string> pathes = await _provider.Files
        .AsNoTracking()
        .Where(file => filesIds.Contains(file.Id))
        .Select(file => file.Path)
        .ToListAsync();

      string query = "Id = ";

      for (int i = 0; i < filesIds.Count; i++)
      {
        query += $"'{filesIds[0]}'";

        if (i != filesIds.Count - 1)
        {
          query += " || Id = ";
        }
      }

      await _context.Database.ExecuteSqlRawAsync($"DELETE FROM Files WHERE {query}");

      return pathes;
    }

    public async Task<List<FileInfo>> GetFileInfoAsync(List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      return await _provider.Files.Where(u => filesIds.Contains(u.Id)).Select(x => _fileInfoMapper.Map(
        x.Path,
        x.Name,
        x.Extension)).ToListAsync();
    }

    public async Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return null;
      }

      return await _provider.Files.Where(u => filesIds.Contains(u.Id)).Select(x => _fileCharacteristicsDataMapper.Map(
        x.Id,
        x.Name,
        x.Extension,
        x.Size,
        x.CreatedAtUtc)).ToListAsync();
    }

    public async Task<bool> EditNameAsync(Guid fileId, string newName)
    {
      DbFile dbFile = await _provider.Files.FirstOrDefaultAsync(p => p.Id == fileId);

      if (dbFile is null)
      {
        return false;
      }

      dbFile.Name = newName;
      dbFile.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbFile.ModifiedAtUtc = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }
  }
}
