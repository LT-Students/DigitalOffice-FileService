using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
    public class DisableFileByIdCommand : IDisableFileByIdCommand
    {
        private readonly IFileRepository _repository;

        public DisableFileByIdCommand(IFileRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Guid fileId)
        {
            _repository.DisableFileById(fileId);
        }
    }
}
