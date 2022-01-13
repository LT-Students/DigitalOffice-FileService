using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;

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

    public async Task<List<FileInfo>> Execute(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return null;
      }

      return (await _repository.GetAsync(filesIds))?.Select(x => _mapper.Map(x)).ToList();
    }
  }
}
