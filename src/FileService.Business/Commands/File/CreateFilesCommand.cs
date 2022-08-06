using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class CreateFilesCommand : ICreateFilesCommand
  {
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IDbFileMapper _mapper;
    private readonly IAccessValidator _accessValidator;
    private readonly IProjectService _projectService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublish _publish;

    public CreateFilesCommand(
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IDbFileMapper mapper,
      IAccessValidator accessValidator,
      IProjectService projectService,
      IHttpContextAccessor httpContextAccessor,
      IPublish publish)
    {
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _mapper = mapper;
      _accessValidator = accessValidator;
      _projectService = projectService;
      _httpContextAccessor = httpContextAccessor;
      _publish = publish;
    }

    public async Task<OperationResultResponse<List<Guid>>> ExecuteAsync(Guid entityId, FileAccessType access, IFormFileCollection uploadedFiles)
    {
      (ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole) = await _projectService.CheckProjectAndUserExistenceAsync(entityId, _httpContextAccessor.HttpContext.GetUserId());
      if (!projectStatus.Equals(ProjectStatusType.Active)
        || !(projectUserRole.HasValue && projectUserRole.Value.Equals(ProjectUserRoleType.Manager))
        && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<List<Guid>> response = new(body: await _fileRepository.
        CreateAsync(uploadedFiles.Select(_mapper.Map).ToList()));

      if (response.Body.Any())
      {
        await _publish.CreateFilesAsync(entityId, access, response.Body);
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
