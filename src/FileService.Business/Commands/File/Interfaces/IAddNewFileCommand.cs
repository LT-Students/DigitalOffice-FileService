using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.File.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a new file.
    /// </summary>
    [AutoInject]
    public interface IAddNewFileCommand
    {
        /// <summary>
        /// Adds a new file. Returns id of the added file.
        /// </summary>
        /// <param name="request">File data.</param>
        /// <returns>Id of the added file.</returns>
        /// <exception cref="ValidationException">Thrown when file data is incorrect.</exception>
        Guid Execute(FileInfo request);
    }
}
