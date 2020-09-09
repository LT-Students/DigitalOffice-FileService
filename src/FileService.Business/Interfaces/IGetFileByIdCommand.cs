using LT.DigitalOffice.FileService.Models.Dto;
using System;

namespace LT.DigitalOffice.FileService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting file model by id.
    /// </summary>
    public interface IGetFileByIdCommand
    {
        /// <summary>
        /// Returns the file model with the specified id.
        /// </summary>
        /// <param name="fileId">Specified id of file.</param>
        /// <returns>File model with specified id.</returns>
        File Execute(Guid fileId);
    }
}
