using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IContentTypeMapper
  {
    string Map(string extension);
  }
}
