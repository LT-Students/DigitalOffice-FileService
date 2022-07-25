using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class GetFilesCommand : IGetFileCommand
  {
    private readonly IFileRepository _repository;
    private readonly IFileInfoMapper _mapper;
    private readonly IProjectService _projectService;
    private readonly IAccessValidator _accessValidator;

    public GetFilesCommand(
        IFileRepository repository,
        IFileInfoMapper mapper,
        IProjectService projectService,
        IAccessValidator accessValidator)
    {
      _repository = repository;
      _mapper = mapper;
      _projectService = projectService;
      _accessValidator = accessValidator;
    }

    public async Task<OperationResultResponse<List<FileInfo>>> ExecuteAsync(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return null;
      }

      OperationResultResponse<List<FileInfo>> response = new();

      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        filesIds = await _projectService.CheckFilesAsync(filesIds, response.Errors);
      }

      response.Body = (await _repository.GetAsync(
       filesIds))?.Select(x => _mapper.Map(x)).ToList();
      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
