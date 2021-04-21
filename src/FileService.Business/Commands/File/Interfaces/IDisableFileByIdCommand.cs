using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for disabling file by id.
    /// </summary>
    [AutoInject]
    public interface IDisableFileByIdCommand
    {
        /// <summary>
        /// Disable the file with the specified id.
        /// </summary>
        /// <param name="fileId">Specified id of file.</param>
        void Execute(Guid fileId);
    }
}
