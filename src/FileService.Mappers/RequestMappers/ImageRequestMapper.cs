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

        public DbImage Map(ImageRequest imageRequest, ImageType imageType)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            byte[] content;
            Guid? parentId;

            if (imageType == ImageType.Full)
            {
                parentId = null;
                content = Convert.FromBase64String(imageRequest.Content);
            }
            else
            {
                parentId = imageRequest.UserId;
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
