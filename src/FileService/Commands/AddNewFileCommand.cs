using FluentValidation;
using LT.DigitalOffice.FileService.Commands.Interfaces;
using LT.DigitalOffice.FileService.Database.Entities;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models;
using LT.DigitalOffice.FileService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Commands
{
    public class AddNewFileCommand : IAddNewFileCommand
    {
        private readonly IFileRepository repository;
        private readonly IValidator<FileCreateRequest> validator;
        private readonly IMapper<FileCreateRequest, DbFile> mapper;

        public AddNewFileCommand(
            [FromServices] IFileRepository repository,
            [FromServices] IValidator<FileCreateRequest> validator,
            [FromServices] IMapper<FileCreateRequest, DbFile> mapper)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public Guid Execute(FileCreateRequest request)
        {
            validator.ValidateAndThrow(request);

            var newFile = mapper.Map(request);

            return repository.AddNewFile(newFile);
        }
    }
}
