using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
    public class AddNewFileCommand : IAddNewFileCommand
    {
        private readonly IFileRepository _repository;
        private readonly IFileInfoValidator _validator;
        private readonly IDbFileMapper _mapper;

        public AddNewFileCommand(
            IFileRepository repository,
            IFileInfoValidator validator,
            IDbFileMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Guid Execute(FileInfo request)
        {
            _validator.ValidateAndThrowCustom(request);

            var newFile = _mapper.Map(request);

            return _repository.AddNewFile(newFile);
        }
    }
}
