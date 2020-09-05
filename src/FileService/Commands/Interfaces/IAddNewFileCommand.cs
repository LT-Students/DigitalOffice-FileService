using LT.DigitalOffice.FileService.Models;
using System;

namespace LT.DigitalOffice.FileService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a new file.
    /// </summary>
    public interface IAddNewFileCommand
    {
        /// <summary>
        /// Adds a new file. Returns id of the added file.
        /// </summary>
        /// <param name="request">File data.</param>
        /// <returns>Id of the added file.</returns>
        Guid Execute(FileCreateRequest request);
    }
}
