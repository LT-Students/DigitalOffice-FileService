using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Helpers;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Validation
{
    public class ImageRequestValidator : AbstractValidator<ImageRequest>, IImageRequestValidator
    {
        public readonly static List<string> AllowedExtensions = new()
            { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tga" };

        public ImageRequestValidator()
        {
            RuleFor(image => image.Content)
                .NotNull().WithMessage("Image content is null or incorrect.")
                .Must(EncodeHelper.IsBase64Coded);

            RuleFor(image => image.Extension)
                .Must(AllowedExtensions.Contains)
                .WithMessage($"Image extension is not {string.Join('/', AllowedExtensions)}");
        }
    }
}
