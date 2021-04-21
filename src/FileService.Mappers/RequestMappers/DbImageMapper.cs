using LT.DigitalOffice.FileService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using SixLabors.ImageSharp;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
{
    public class DbImageMapper : IDbImageMapper
    {
        public IImageResizeAlgorithm _resizeAlgotithm;

        public DbImageMapper(
            IImageResizeAlgorithm resizeAlgotithm)
        {
            _resizeAlgotithm = resizeAlgotithm;
        }

        public DbImage Map(
            ImageRequest imageRequest,
            ImageType imageType,
            out bool isBigImage,
            Guid userId,
            Guid? parentId = null)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            string content;

            if (imageType == ImageType.Full)
            {
                byte[] byteString = Convert.FromBase64String(imageRequest.Content);

                Image image = Image.Load(byteString);
                isBigImage = image.Width > 150 && image.Height > 150;
                content = imageRequest.Content;

                if (parentId != null)
                {
                    throw new ArgumentException("If image type is Full then parentId must be null.");
                }
            }
            else
            {
                isBigImage = false;
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
                UserId = userId,
                ImageType = (int)imageType,
                IsActive = true
            };
        }
    }
}
