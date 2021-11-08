using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Mappers.PatchDocument.Interfaces
{
  [AutoInject]
  public interface IPatchDbFileMapper
  {
    JsonPatchDocument<DbFile> Map(JsonPatchDocument<EditFileRequest> request);
  }
}
