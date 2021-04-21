using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbImageMapper
    {
        DbImage Map(ImageRequest imageRequest, ImageType imageType, out bool isBigImage, Guid userId, Guid? parentId = null);
    }
}
