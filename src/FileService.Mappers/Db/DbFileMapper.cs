using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models.File;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Db
{
    public class DbFileMapper : IDbFileMapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbFileMapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbFile Map(FileData file, Guid id)
        {
            if (file == null)
            {
                return null;
            }

            return new DbFile()
            {
                Id = Guid.NewGuid(),
                Content = file.Content,
                Extension = file.Extension.ToLower(),
                Name = file.Name,
                CreatedBy = id,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
            };
        }
    }
}