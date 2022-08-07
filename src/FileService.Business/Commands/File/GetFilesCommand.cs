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
    private readonly IContentTypeMapper _contentTypeMapper;

    public GetFilesCommand(
        IFileRepository repository,
        IProjectService projectService,
        IAccessValidator accessValidator,
        IContentTypeMapper contentTypeMapper)
    {
      _repository = repository;
      _projectService = projectService;
      _accessValidator = accessValidator;
      _contentTypeMapper = contentTypeMapper;
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

      return (await _repository.GetAsync(
        filesIds))?.Select(file => (
          Convert.FromBase64String(file.Content),
          _contentTypeMapper.Map(file.Extension),
          file.Name)).ToList();
    }
  }
}
