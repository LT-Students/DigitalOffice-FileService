using System;
using System.IO;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
  public class DbFileMapper : IDbFileMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbFileMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbFile Map(IFormFile uploadedFile)
    {
      if (uploadedFile is null)
      {
        return null;
      }

      using MemoryStream ms = new();
      uploadedFile.CopyTo(ms);

      return new DbFile()
      {
        Id = Guid.NewGuid(),
        Content = Convert.ToBase64String(ms.ToArray()),
        Extension = Path.GetExtension(uploadedFile.FileName),
        Name = Path.GetFileNameWithoutExtension(uploadedFile.FileName),
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
