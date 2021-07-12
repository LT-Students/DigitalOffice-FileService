using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Data.Interfaces
{
    /// <summary>
    /// Represents interface of repository in repository pattern.
    /// Provides methods for working with the database of FileService.
    /// </summary>
    [AutoInject]
    public interface IImageRepository
    {
        /// <summary>
        /// Adds new image to the database. Returns the id of the added image.
        /// </summary>
        /// <param name="dbImage">Image to add.</param>
        /// <returns>Id of the added image.</returns>
        Guid Add(DbImage dbImage);

        DbImage Get(Guid imageId);

        List<DbImage> Get(List<Guid> imageIds);
    }
}
