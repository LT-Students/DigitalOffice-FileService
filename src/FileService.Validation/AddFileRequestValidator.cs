using FluentValidation;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using MimeTypes;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Validation
{
  public class AddFileRequestValidator : AbstractValidator<IFormFileCollection>, IAddFileRequestValidator
  {
    public AddFileRequestValidator()
    {
      RuleForEach(file => file)
        .Must(x => x.FileName.Length < 245).WithMessage("File name is too long")
        .Must(x => MimeTypeMap.GetExtension(x.ContentType).Length < 10).WithMessage("File extension is too long");
    }
  }
}
