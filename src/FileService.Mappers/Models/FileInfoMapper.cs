using System;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
  public class FileInfoMapper : IFileInfoMapper
  {
    public FileInfo Map(Guid id, string name, string extension, DateTime modifiedAtUtc)
    {
      return new FileInfo
      {
        Id = id,
        Name = name,
        Extension = extension,
        ModifiedAtUtc = modifiedAtUtc
      };
    }
  }
}
