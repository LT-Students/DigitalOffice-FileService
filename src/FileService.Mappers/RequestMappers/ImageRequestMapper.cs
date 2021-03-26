using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
{
    public class ImageRequestMapper : IImageRequestMapper
    {
        public IImageResizeAlgorithm _resizeAlgotithm;

        public ImageRequestMapper([FromServices] IImageResizeAlgorithm resizeAlgotithm)
        {
            _resizeAlgotithm = resizeAlgotithm;
        }

        public DbImage Map(ImageRequest imageRequest, ImageType imageType, Guid? parentId = null)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            byte[] content;
            if (imageType == ImageType.Full)
            {
                content = Convert.FromBase64String(imageRequest.Content);

                if (parentId != null)
                {
                    throw new ArgumentException("If image type is Full then parentId must be null.");
                }
            }
            else
            {
                content = _resizeAlgotithm.Resize(imageRequest.Content, imageRequest.Extension);
            }

            return new DbImage()
            {
                Id = Guid.NewGuid(),
                ParentId = parentId,
                Content = content,
                Extension = imageRequest.Extension.ToLower(),
                Name = imageRequest.Name,
                AddedOn = DateTime.Now,
                UserId = imageRequest.UserId,
                ImageType = (int)imageType,
                IsActive = true
            }; ;
        }
    }
}
