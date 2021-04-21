using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Validation.Interfaces
{
    [AutoInject]
    public interface IFileInfoValidator : IValidator<FileInfo>
    {
    }
}
