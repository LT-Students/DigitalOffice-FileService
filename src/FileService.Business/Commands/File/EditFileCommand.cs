using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.ProjectService.Broker.Requests.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class EditFileCommand : IEditFileCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IPatchDbFileMapper _mapper;
    private readonly IEditFileRequestValidator _requestValidator;
    private readonly IProjectService _projectService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditFileCommand(
      IAccessValidator accessValidator,
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IPatchDbFileMapper mapper,
      IEditFileRequestValidator requestValidator,
      IProjectService projectService,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _mapper = mapper;
      _requestValidator = requestValidator;
      _projectService = projectService;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, string name)
    {
      (ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole) = await _projectService.GetProjectUserRole(entityId, _httpContextAccessor.HttpContext.GetUserId());
      if (!projectStatus.Equals(ProjectStatusType.Active)
        || !(projectUserRole.HasValue && projectUserRole.Value.Equals(ProjectUserRoleType.Manager))
        && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      OperationResultResponse<bool> response = new(
        body: await _fileRepository.EditNameAsync(fileId, name),
        status: OperationResultStatusType.FullSuccess);

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
