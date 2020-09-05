using LT.DigitalOffice.FileService.Database;
using LT.DigitalOffice.FileService.Database.Entities;
using LT.DigitalOffice.FileService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LT.DigitalOffice.FileService.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileServiceDbContext dbContext;

        public FileRepository([FromServices] FileServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Guid AddNewFile(DbFile file)
        {
            dbContext.Files.Add(file);
            dbContext.SaveChanges();

            return file.Id;
        }

        public DbFile GetFileById(Guid fileId)
        {
            var dbFile = dbContext.Files.FirstOrDefault(file => file.Id == fileId);

            if (dbFile == null)
            {
                throw new Exception("File with this id was not found.");
            }

            return dbFile;
        }
    }
}
