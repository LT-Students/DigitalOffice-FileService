using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.FileService.Broker.Requests.Interfaces;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class CreateFilesCommand : ICreateFilesCommand
  {
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IAccessValidator _accessValidator;
    private readonly IProjectService _projectService;
    private readonly IWikiService _wikiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublish _publish;
    private readonly IDbFileMapper _mapper;

    public CreateFilesCommand(
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IAccessValidator accessValidator,
      IProjectService projectService,
      IWikiService wikiService,
      IHttpContextAccessor httpContextAccessor,
      IPublish publish,
      IDbFileMapper mapper)
    {
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _accessValidator = accessValidator;
      _projectService = projectService;
      _wikiService = wikiService;
      _httpContextAccessor = httpContextAccessor;
      _publish = publish;
      _mapper = mapper;
    }

    public async Task<OperationResultResponse<List<Guid>>> ExecuteAsync(
      Guid entityId,
      ServiceType serviceType,
      FileAccessType access,
      IFormFileCollection uploadedFiles)
    {
      if (serviceType == ServiceType.Project)
      {
        (ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole) = await _projectService.GetProjectUserRole(entityId, _httpContextAccessor.HttpContext.GetUserId());
        if (!projectStatus.Equals(ProjectStatusType.Active)
          || !(projectUserRole.HasValue && projectUserRole.Value.Equals(ProjectUserRoleType.Manager))
          && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
        {
          return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
        }
      }
      else if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki)
        || !_wikiService.CheckArticlesAsync(new List<Guid> { entityId }).Result.Any())
      {
        return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<List<Guid>> response = new(body: await _fileRepository.
        CreateAsync(uploadedFiles.Select(_mapper.Map).ToList()));

      if (response.Body.Any())
      {
        switch (serviceType)
        {
          case ServiceType.Project:
            await _publish.CreateFilesAsync(entityId, access, response.Body);
            break;
          case ServiceType.Wiki:
            await _publish.CreateWikiFilesAsync(entityId, response.Body);
            break;
        }

        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      }
      else
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      }

      return response;
    }
  }
}
