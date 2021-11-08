using System;
using LT.DigitalOffice.FileService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.FileService.Mappers.PatchDocument
{
  public class PatchDbFileMapper : IPatchDbFileMapper
  {
    public JsonPatchDocument<DbFile> Map(JsonPatchDocument<EditFileRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbFile> dbRequest = new();

      foreach (var item in request.Operations)
      {
        dbRequest.Operations.Add(new Operation<DbFile>(item.op, item.path, item.from, item.value));
      }

      return dbRequest;
    }
  }
}
