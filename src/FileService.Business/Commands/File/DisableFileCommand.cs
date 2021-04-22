using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
    public class DisableFileCommand : IDisableFileCommand
    {
        private readonly IFileRepository _repository;

        public DisableFileCommand(IFileRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Guid fileId)
        {
            _repository.DisableFileById(fileId);
        }
    }
}
