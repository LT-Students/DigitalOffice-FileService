using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
    public class DbFileMapper : IDbFileMapper
    {
        public DbFile Map(AddFileRequest file)
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
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
            };
        }
    }
}