using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using System;
using System.Linq;

namespace LT.DigitalOffice.FileService.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly IDataProvider provider;

        public FileRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public Guid AddNewFile(DbFile file)
        {
            provider.Files.Add(file);
            provider.Save();

            return file.Id;
        }

        public void DeleteFileById(Guid fileId)
        {
            var dbFile = provider.Files.FirstOrDefault(file => file.Id == fileId);

            if (dbFile == null)
            {
                throw new NotFoundException("File with this id was not found.");
            }

            provider.Files.Remove(dbFile);
            provider.Save();
        }

        public DbFile GetFileById(Guid fileId)
        {
            var dbFile = provider.Files.FirstOrDefault(file => file.Id == fileId);

            if (dbFile == null)
            {
                throw new NotFoundException("File with this id was not found.");
            }

            return dbFile;
        }
    }
}
