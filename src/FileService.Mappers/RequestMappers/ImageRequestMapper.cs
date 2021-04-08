using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
{
    public class ImageRequestMapper : IImageRequestMapper
    {
        public IImageResizeAlgorithm _resizeAlgotithm;
        public HttpContext _httpContext;

        public ImageRequestMapper(
            IImageResizeAlgorithm resizeAlgotithm,
            IHttpContextAccessor httpContextAccessor)
        {
            _resizeAlgotithm = resizeAlgotithm;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public DbImage Map(ImageRequest imageRequest, ImageType imageType, out bool isBigImage, Guid? parentId = null)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            string content;

            if (imageType == ImageType.Full)
            {
                using Image image = Image.Load(imageRequest.Content);
                isBigImage = image.Width > 150 && image.Height > 150;
                content = imageRequest.Content;

                if (parentId != null)
                {
                    throw new ArgumentException("If image type is Full then parentId must be null.");
                }
            }
            else
            {
                isBigImage =false;
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
                UserId = _httpContext.GetUserId(),
                ImageType = (int)imageType,
                IsActive = true
            };
        }
    }
}
