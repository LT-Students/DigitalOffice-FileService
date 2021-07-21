using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Helpers;
using LT.DigitalOffice.FileService.Validation.Interfaces;

namespace LT.DigitalOffice.FileService.Validation
{
    public class FileRequestValidator : AbstractValidator<AddFileRequest>, IFileRequestValidator
    {
        public FileRequestValidator()
        {
            RuleFor(file => file.Name)
                .NotEmpty()
                .MaximumLength(244).WithMessage("File name is too long")
                .Matches("^[A-Z 0-9][A-Z a-z 0-9]+$");

            RuleFor(file => file.Content)
                .NotNull().WithMessage("The file is null")
                .Must(EncodeHelper.IsBase64Coded);
        }
    }
}