﻿using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Validation.Helpers;

namespace LT.DigitalOffice.FileService.Validation
{
    public class FileValidator : AbstractValidator<File>
    {
        public FileValidator()
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