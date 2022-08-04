using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IFileCharacteristicsDataMapper
  {
    FileCharacteristicsData Map(Guid id, string name, string extension, long size, DateTime createdAtUtc);
  }
}
