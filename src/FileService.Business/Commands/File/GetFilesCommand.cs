using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class GetFilesCommand : IGetFilesCommand
  {
    private readonly IFileRepository _repository;
    private readonly IProjectService _projectService;
    private readonly IAccessValidator _accessValidator;
    private readonly IContentTypeMapper _mapper;

    public GetFilesCommand(
        IFileRepository repository,
        IProjectService projectService,
        IAccessValidator accessValidator,
        IContentTypeMapper mapper)
    {
      _repository = repository;
      _projectService = projectService;
      _accessValidator = accessValidator;
      _mapper = mapper;
    }

    public async Task<List<(byte[] content, string extension, string name)>> ExecuteAsync(List<Guid> filesIds)
    {
      if (filesIds is null)
      {
        return null;
      }

      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        filesIds = await _projectService.CheckFilesAsync(filesIds);
      }

      var r = await _repository.GetAsync(
        filesIds);

      return (r)?.Select(file => (
          file.FileStream,
          _mapper.Map(".png"),
          file.Name)).ToList();
    }
  }
}
