using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
    [AutoInject]
    public interface IGetFileCommand
    {
        Task<List<FileData>> Execute(List<Guid> filesIds);
    }
}
