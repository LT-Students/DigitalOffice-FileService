using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Broker.Requests.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using FileInfo = LT.DigitalOffice.FileService.Models.Dto.Models.FileInfo;
using LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces;
using System.IO;

namespace LT.DigitalOffice.FileService.Business.Commands.Files
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

      List<(byte[] content, string extension, string name)> result = new();
      List<FileInfo> files = await _repository.GetFileInfoAsync(filesIds);

      foreach (FileInfo fileInfo in files)
      {
        result.Add((await File.ReadAllBytesAsync(fileInfo.Path), _mapper.Map(fileInfo.Extension), fileInfo.Name));
      }

      return result;
    }
  }
}
