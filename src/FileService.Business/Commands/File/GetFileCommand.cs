using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class GetFileCommand : IGetFileCommand
  {
    private readonly IFileRepository _repository;
    private readonly IFileInfoMapper _mapper;
    private readonly IProjectService _projectService;

    public GetFileCommand(
        IFileRepository repository,
        IFileInfoMapper mapper,
        IProjectService projectService)
    {
      _repository = repository;
      _mapper = mapper;
      _projectService = projectService;
    }

    public async Task<OperationResultResponse<List<FileInfo>>> Execute(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return null;
      }

      OperationResultResponse<List<FileInfo>> response = new();

      response.Body = (await _repository.GetAsync(
          await _projectService.CheckFilesAsync(filesIds, response.Errors)))?.Select(x => _mapper.Map(x)).ToList();
      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
