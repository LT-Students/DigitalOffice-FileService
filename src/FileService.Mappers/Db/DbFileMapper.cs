using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
    public class DbFileMapper : IDbFileMapper
    {
        public DbFile Map(FileData file, Guid createdBy)
        {
            if (file == null)
            {
                return null;
            }

            return new DbFile()
            {
                Id = file.Id,
                Content = file.Content,
                Extension = file.Extension.ToLower(),
                Name = file.Name,
                CreatedBy = createdBy,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
