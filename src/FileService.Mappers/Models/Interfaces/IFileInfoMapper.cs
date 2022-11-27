using System;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IFileInfoMapper
  {
    FileInfo Map(string path, string name, string extension);
  }
}
