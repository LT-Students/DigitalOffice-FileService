using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data
{
  public class FileRepository : IFileRepository
  {
    private readonly IDataProvider _provider;
    private readonly IFileCharacteristicsDataMapper _fileCharacteristicsDataMapper;

    public FileRepository(
      IDataProvider provider,
      IFileCharacteristicsDataMapper fileCharacteristicsDataMapper)
    {
      _provider = provider;
      _fileCharacteristicsDataMapper = fileCharacteristicsDataMapper;
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

    public async Task<List<Guid>> RemoveAsync(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return filesIds;
      }

      IEnumerable<DbFile> files = await _provider.Files
        .Where(x => filesIds.Contains(x.Id)).ToListAsync();

      _provider.Files.RemoveRange(files);
      await _provider.SaveAsync();

      return files.Select(f => f.Id).ToList();
    }

    public async Task<List<DbFile>> GetAsync(List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      string query = "stream_id = ";
      
      for (int i = 0; i < filesIds.Count; i++)
      {
        query += $"'{filesIds[0]}'";

        if (i != filesIds.Count - 1)
        {
          query += " && stream_id = ";
        }  
      }

      return await _provider.Files.FromSqlRaw($"SELECT * FROM Files WITH (READCOMMITTEDLOCK) where {query}").ToListAsync();

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
        x.FileType,
        x.CachedFileSize.Value,
        x.CreationTime.UtcDateTime)).ToListAsync();
    }

    public async Task<bool> EditNameAsync(Guid fileId, string newName)
    {
      DbFile dbFile = await _provider.Files.FirstOrDefaultAsync(p => p.Id == fileId);

      if (dbFile is null)
      {
        return false;
      }

      dbFile.Name = newName;
      dbFile.LastWriteTime = DateTime.UtcNow;

      await _provider.SaveAsync();

      return true;
    }
  }
}
