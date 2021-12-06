using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Validation.Interfaces
{
  [AutoInject]
  public interface IEditFileRequestValidator : IValidator<JsonPatchDocument<EditFileRequest>>
  {
  }
}
