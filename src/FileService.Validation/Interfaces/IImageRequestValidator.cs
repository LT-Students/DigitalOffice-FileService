using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Validation.Interfaces
{
    [AutoInject]
    public interface IImageRequestValidator : IValidator<ImageRequest>
    {
    }
}
