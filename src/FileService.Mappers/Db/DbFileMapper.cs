using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
    public class DbFileMapper : IDbFileMapper
    {
        public DbFile Map(FileRequest file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return new DbFile()
            {
                Id = Guid.NewGuid(),
                Content = file.Content,
                Extension = file.Extension.ToLower(),
                Name = file.Name,
                AddedOn = DateTime.UtcNow,
                IsActive = true,
            };
        }
    }
}