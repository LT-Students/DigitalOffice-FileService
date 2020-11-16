using FluentValidation;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    /// <inheritdoc cref="IAddNewFileCommand"/>
    public class AddNewFileCommand : IAddNewFileCommand
    {
        private readonly IFileRepository repository;
        private readonly IValidator<File> validator;
        private readonly IMapper<File, DbFile> mapper;

        public AddNewFileCommand(
            [FromServices] IFileRepository repository,
            [FromServices] IValidator<File> validator,
            [FromServices] IMapper<File, DbFile> mapper)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public Guid Execute(File request)
        {
            validator.ValidateAndThrowCustom(request);

            var newFile = mapper.Map(request);

            return repository.AddNewFile(newFile);
        }
    }
}
