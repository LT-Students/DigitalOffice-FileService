using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting file model by id.
    /// </summary>
    [AutoInject]
    public interface IGetFileByIdCommand
    {
        /// <summary>
        /// Returns the file model with the specified id.
        /// </summary>
        /// <param name="fileId">Specified id of file.</param>
        /// <returns>File model with specified id.</returns>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when file not found.</exception>
        FileInfo Execute(Guid fileId);
    }
}
