using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Business.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a new image.
    /// </summary>
    public interface IAddNewImageCommand
    {
        /// <summary>
        /// Adds the new image and adds thumb (150*150) image. Returns id of the added original image.
        /// </summary>
        /// <param name="request">Image data.</param>
        /// <param name="userId">UserId if it is request from broker, else null.</param>
        /// <returns>Id of the added original image.</returns>
        Guid Execute(ImageRequest request, Guid? userId = null);
    }
}
