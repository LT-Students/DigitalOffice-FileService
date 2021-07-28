using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
    public class ImageInfoMapper : IImageInfoMapper
    {
        public ImageInfo Map(DbImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return new ImageInfo
            {
                Id = image.Id,
                Name = image.Name,
                Type = (ImageType)image.ImageType,
                ParentId = image.ParentId,
                Content = image.Content,
                Extension = image.Extension
            };
        }
    }
}
