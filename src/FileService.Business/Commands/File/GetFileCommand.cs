using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
    public class GetFileCommand : IGetFileCommand
    {
        private readonly IFileRepository _repository;
        private readonly IFileDataMapper _mapper;

        public GetFileCommand(
            IFileRepository repository,
            IFileDataMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FileData>> Execute(List<Guid> filesIds)
        {
            return (await _repository.GetAsync(filesIds)).Select(x => _mapper.Map(x)).ToList();
        }
    }
}