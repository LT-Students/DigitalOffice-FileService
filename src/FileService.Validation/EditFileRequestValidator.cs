using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.FileService.Validation
{
  public class EditFileRequestValidator : BaseEditRequestValidator<EditFileRequest>, IEditFileRequestValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditFileRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditFileRequest.Name),
          nameof(EditFileRequest.Content)
        });

      AddСorrectOperations(nameof(EditFileRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditFileRequest.Content), new List<OperationType> { OperationType.Replace });

      #endregion
    }

    public EditFileRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
