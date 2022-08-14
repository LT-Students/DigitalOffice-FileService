using System;
using System.IO;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.AspNetCore.Http;
using MimeTypes;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
  public class DbFileMapper : IDbFileMapper
  {
    public DbFile Map(IFormFile uploadedFile)
    {
      if (uploadedFile is null)
      {
        return null;
      }

      using MemoryStream ms = new();
      uploadedFile.CopyTo(ms);

      return new()
      {
        Id = Guid.NewGuid(),
        FileStream = ms.ToArray(),
        FileType = MimeTypeMap.GetExtension(uploadedFile.ContentType),
        Name = uploadedFile.FileName,
        CreationTime = DateTime.UtcNow
      };
    }
  }
}
