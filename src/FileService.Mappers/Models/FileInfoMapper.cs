using System;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
  public class FileInfoMapper : IFileInfoMapper
  {
    public FileInfo Map(string path, string name, string extension)
    {
      return new FileInfo
      {
        Path = path,
        Name = name,
        Extension = extension
      };
    }
  }
}
