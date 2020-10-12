using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    class DeleteFileByIdRepositoryTests
    {
        private FileServiceDbContext dbContext;
        private IFileRepository repository;
        private Guid fileId;
        private DbFile dbFile;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<FileServiceDbContext>()
                                    .UseInMemoryDatabase("InMemoryDatabase")
                                    .Options;
            dbContext = new FileServiceDbContext(dbOptions);
            repository = new FileRepository(dbContext);

            fileId = Guid.NewGuid();
            dbFile = new DbFile
            {
                Id = fileId,
                Content = Convert.FromBase64String("RGlnaXRhbCBPZmA5Y2U="),
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };

            dbContext.Files.Add(dbFile);
            dbContext.SaveChanges();
        }

        [TearDown]
        public void Clean()
        {
            if (dbContext.Database.IsInMemory())
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenFileWasNotFound()
        {
            Assert.Throws<NotFoundException>(() => repository.DeleteFileById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldDeleteFile()
        {
            repository.DeleteFileById(dbFile.Id);

            Assert.That(dbContext.Files.Find(fileId) == null);
        }
    }
}
