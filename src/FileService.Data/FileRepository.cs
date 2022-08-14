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
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data
{
  public class FileRepository : IFileRepository
  {
    private readonly IDataProvider _provider;
    private readonly IFileCharacteristicsDataMapper _fileCharacteristicsDataMapper;
    private readonly IFileInfoMapper _fileInfoMapper;
    private readonly FileServiceDbContext _context;

    public FileRepository(
      IDataProvider provider,
      IFileCharacteristicsDataMapper fileCharacteristicsDataMapper,
      IFileInfoMapper fileInfoMapper,
      FileServiceDbContext context)
    {
      _provider = provider;
      _fileCharacteristicsDataMapper = fileCharacteristicsDataMapper;
      _fileInfoMapper = fileInfoMapper;
      _context = context;
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
      FileInfo file = await _provider.Files.Where(u => u.Id == fileId).Select(x => _fileInfoMapper.Map(
        x.Id,
        x.Name,
        x.FileType,
        x.LastWriteTime.UtcDateTime)).FirstOrDefaultAsync();

      if (file is null)
      {
        return false;
      }

      _context.Database.ExecuteSqlRaw("UPDATE Files SET name = {0}, last_write_time = {1} WHERE stream_id = {2}", newName + file.Extension, DateTime.UtcNow, fileId);

      return true;
    }
  }
}
