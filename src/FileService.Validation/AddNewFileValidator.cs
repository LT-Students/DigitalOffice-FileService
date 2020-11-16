﻿using FluentValidation;
using LT.DigitalOffice.FileService.Models.Dto;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.FileService.Validation
{
    public class AddNewFileValidator : AbstractValidator<File>
    {
        public AddNewFileValidator()
        {
            RuleFor(file => file.Name)
                .NotEmpty()
                .MaximumLength(244).WithMessage("File name is too long")
                .Matches("^[A-Z 0-9][A-Z a-z 0-9]+$");

            RuleFor(file => file.Content)
                .NotNull().WithMessage("The file is null")
                .Must(IsBase64Coded);
        }

        private bool IsBase64Coded(string base64String)
        {
            if (base64String == null)
            {
                return false;
            }

            var byteString = new Span<byte>(new byte[base64String.Length]);
            return Convert.TryFromBase64String(base64String, byteString, out _);
        }
    }
}