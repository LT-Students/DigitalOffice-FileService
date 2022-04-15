using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.FileService.Business.Commands.File
{
  public class EditFileCommand : IEditFileCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IPatchDbFileMapper _mapper;
    private readonly IEditFileRequestValidator _requestValidator;

    public EditFileCommand(
      IAccessValidator accessValidator,
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IPatchDbFileMapper mapper,
      IEditFileRequestValidator requestValidator)
    {
      _accessValidator = accessValidator;
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _mapper = mapper;
      _requestValidator = requestValidator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid fileId, JsonPatchDocument<EditFileRequest> request)
    {
      OperationResultResponse<bool> response = new();

      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult result = _requestValidator.Validate(request);

      if (!result.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest,
          result.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      response.Body = await _fileRepository.EditAsync(fileId, _mapper.Map(request));

      response.Status = OperationResultStatusType.FullSuccess;
      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
