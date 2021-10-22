using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            _provider.Save();
        }

        public async Task<List<DbFile>> GetAsync(List<Guid> filesIds)
        {
            return await _provider.Files.Where(u => filesIds.Contains(u.Id) && u.IsActive).ToListAsync();
        }
    }
}
