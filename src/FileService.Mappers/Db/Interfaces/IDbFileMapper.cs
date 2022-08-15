using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbFileMapper
  {
    DbFile Map(IFormFile uploadedFile);
  }
}
