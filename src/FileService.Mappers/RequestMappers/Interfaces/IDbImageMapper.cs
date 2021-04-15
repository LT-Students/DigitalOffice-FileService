using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces
{
    public interface IDbImageMapper
    {
        DbImage Map(ImageRequest imageRequest, ImageType imageType, out bool isBigImage, Guid userId, Guid? parentId = null);
    }
}
