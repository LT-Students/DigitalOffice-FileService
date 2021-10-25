using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
    public class FileInfoMapper : IFileInfoMapper
    {
        public FileInfo Map(DbFile dbFile)
        {
            if (dbFile == null)
            {
                return null;
            }

            return new FileInfo()
            {
                Id = dbFile.Id,
                Content = dbFile.Content,
                Extension = dbFile.Extension,
                Name = dbFile.Name
            };
        }
    }
}
