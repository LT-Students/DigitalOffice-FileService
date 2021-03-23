using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
{
    public class ImageRequestMapper : IMapper<ImageRequest, DbImage>
    {
        public DbImage Map(ImageRequest imageRequest)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            return new DbImage()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = imageRequest.Extension.ToLower(),
                Name = imageRequest.Name,
                AddedOn = DateTime.Now,
                UserId = imageRequest.UserId,
                ImageType = (int)ImageType.Full,
                IsActive = true
            };
        }
    }
}
