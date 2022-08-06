﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data
{
  public class FileRepository : IFileRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileCharacteristicsDataMapper _fileCharacteristicsDataMapper;

    public FileRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor,
      IFileCharacteristicsDataMapper fileCharacteristicsDataMapper)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
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
      if (filesIds is null)
      {
        return null;
      }

      return await _provider.Files.Where(u => filesIds.Contains(u.Id)).ToListAsync();
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

    public async Task<bool> EditAsync(Guid fileId, JsonPatchDocument<DbFile> request)
    {
      DbFile dbFile = await _provider.Files.FirstOrDefaultAsync(p => p.Id == fileId);

      if (dbFile is null)
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
