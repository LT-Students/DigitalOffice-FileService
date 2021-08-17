using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
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
                CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
            };
        }
    }
}