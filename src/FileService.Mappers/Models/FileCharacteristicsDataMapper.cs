using System;
using System.IO;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
  public class FileCharacteristicsDataMapper : IFileCharacteristicsDataMapper
  {
    public FileCharacteristicsData Map(Guid id, string name, string extension, long size, DateTime createdAtUtc)
    {
      return new(
        id: id,
        name: Path.GetFileNameWithoutExtension(name),
        extension: extension,
        size: size,
        createdAtUtc: createdAtUtc);
    }
  }
}
