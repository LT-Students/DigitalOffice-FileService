using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class EditFileCommand : IEditFileCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IFileRepository _fileRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;
    private readonly IPatchDbFileMapper _mapper;

    public EditFileCommand(
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreater,
      IFileRepository fileRepository,
      IPatchDbFileMapper mapper)
    {
      _accessValidator = accessValidator;
      _fileRepository = fileRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreater = responseCreater;
      _mapper = mapper;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid fileId, JsonPatchDocument<EditFileRequest> request)
    {
      OperationResultResponse<bool> response = new();

      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      response.Body = await _fileRepository.EditAsync(fileId, _mapper.Map(request));

      response.Status = OperationResultStatusType.FullSuccess;
      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        response.Errors.Add("File can not be edit.");
      }

      return response;
    }
  }
}
