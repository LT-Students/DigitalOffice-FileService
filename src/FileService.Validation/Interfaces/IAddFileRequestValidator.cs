using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Validation.Interfaces
{
    [AutoInject]
    public interface IAddFileRequestValidator : IValidator<IFormFileCollection>
    {
    }
}
