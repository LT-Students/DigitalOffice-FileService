using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces
{
    public interface IImageRequestMapper
    {
        public DbImage Map(ImageRequest imageRequest, ImageType imageType);
    }
}
