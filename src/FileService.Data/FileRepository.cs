using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Constants;
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

    private string GetTargetDBTableName(FileSource fileSource)
    {
      switch (fileSource)
      {
        case FileSource.Wiki: return DBTablesNames.Wiki;
        case FileSource.Project: return DBTablesNames.PROJECT;
        default: throw new ArgumentOutOfRangeException();
      }
    }

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

    public async Task<List<Guid>> CreateAsync(FileSource fileSource, List<DbFile> files)
    {
      if (files is not null && files.Any())
      {
        foreach (DbFile file in files)
        {
          await _provider.ExecuteRawSqlAsync(
            @$"INSERT INTO {GetTargetDBTableName(fileSource)}
              (Id, Name, Extension, Size, Path, CreatedAtUtc, CreatedBy)
              VALUES ('{file.Id}', '{file.Name}', '{file.Extension}', '{file.Size}', '{file.Path}', '{file.CreatedAtUtc.ToString("yyyy-MM-dd HH:mm:ss")}', '{file.CreatedBy}')");
        }
      }

      return files.Select(x => x.Id).ToList();
    }

    public async Task<List<string>> RemoveAsync(FileSource fileSource, List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      string tableName = GetTargetDBTableName(fileSource);

      List<string> pathes = await _provider
        .FromSqlRaw($"SELECT * FROM {tableName}")
        .AsNoTracking()
        .Where(x => filesIds.Contains(x.Id))
        .Select(x => x.Path)
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

      await _context.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName} WHERE {query}");

      return pathes;
    }

    public async Task<List<FileInfo>> GetFileInfoAsync(FileSource fileSource, List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      return await _provider
        .FromSqlRaw($"SELECT * FROM {GetTargetDBTableName(fileSource)}")
        .AsNoTracking()
        .Where(u => filesIds.Contains(u.Id))
        .Select(x => _fileInfoMapper.Map(
          x.Path,
          x.Name,
          x.Extension)).ToListAsync();
    }

    public async Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(FileSource fileSource, List<Guid> filesIds)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      return await _provider
        .FromSqlRaw($"SELECT * FROM {GetTargetDBTableName(fileSource)}")
        .AsNoTracking()
        .Where(u => filesIds.Contains(u.Id))
        .Select(x => _fileCharacteristicsDataMapper.Map(
          x.Id,
          x.Name,
          x.Extension,
          x.Size,
          x.CreatedAtUtc)).ToListAsync();
    }

    public async Task<bool> EditNameAsync(FileSource fileSource, Guid fileId, string newName)
    {
      await _provider.ExecuteRawSqlAsync(
        $"UPDATE {GetTargetDBTableName(fileSource)} SET Name = '{newName}', ModifiedBy = '{_httpContextAccessor.HttpContext.GetUserId()}', ModifiedAtUtc = '{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}'");

      return true;
    }
  }
}
