using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
{
    public class ImageRequestMapper : IMapper<ImageRequest, DbImage>
    {
        public DbImage Map(ImageRequest image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return new DbImage()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Content = Convert.FromBase64String(image.Content),
                Extension = image.Extension.ToLower(),
                Name = image.Name,
                AddedOn = DateTime.Now,
                ImageType = (int)ImageType.Full,
                IsActive = true
            };
        }
    }
}
