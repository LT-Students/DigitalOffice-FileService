using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
    [AutoInject]
    public interface IGetFileCommand
    {
        Task<OperationResultResponse<List<FileInfo>>> Execute(List<Guid> filesIds);
    }
}
