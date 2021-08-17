using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.FileService.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly IDataProvider _provider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileRepository(IDataProvider provider, IHttpContextAccessor httpContextAccessor)
        {
            _provider = provider;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid AddFile(DbFile file)
        {
            _provider.Files.Add(file);
            _provider.Save();

            return file.Id;
        }

        public void DisableFile(Guid fileId)
        {
            var dbFile = _provider.Files.FirstOrDefault(file => file.Id == fileId);

            if (dbFile == null)
            {
                throw new NotFoundException("File with this id was not found.");
            }

            dbFile.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
            dbFile.ModifiedAtUtc = DateTime.UtcNow;
            dbFile.IsActive = false;

            _provider.Files.Update(dbFile);
            _provider.Save();
        }

        public DbFile GetFile(Guid fileId)
        {
            var dbFile = _provider.Files.FirstOrDefault(file => file.Id == fileId);

            if (dbFile == null)
            {
                throw new NotFoundException("File with this id was not found.");
            }

            return dbFile;
        }
    }
}
