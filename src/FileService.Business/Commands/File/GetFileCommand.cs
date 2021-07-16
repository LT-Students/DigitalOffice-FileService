using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
    public class GetFileCommand : IGetFileCommand
    {
        private readonly IFileRepository _repository;
        private readonly IFileInfoMapper _mapper;

        public GetFileCommand(
            IFileRepository repository,
            IFileInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public FileInfo Execute(Guid fileId)
        {
            return _mapper.Map(_repository.GetFile(fileId));
        }
    }
}