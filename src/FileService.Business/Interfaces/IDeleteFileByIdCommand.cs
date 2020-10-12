using System;

namespace LT.DigitalOffice.FileService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for deleting file by id.
    /// </summary>
    public interface IDeleteFileByIdCommand
    {
        /// <summary>
        /// Deletes the file with the specified id.
        /// </summary>
        /// <param name="fileId">Specified id of file.</param>
        void Execute(Guid fileId);
    }
}
