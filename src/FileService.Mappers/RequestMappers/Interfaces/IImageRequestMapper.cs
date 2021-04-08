using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces
{
    public interface IImageRequestMapper
    {
        public DbImage Map(ImageRequest imageRequest, ImageType imageType, out bool isBigImage, Guid? parentId = null);
    }
}
