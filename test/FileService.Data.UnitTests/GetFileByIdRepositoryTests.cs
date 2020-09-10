using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class GetFileByIdRepositoryTests
    {
        private FileServiceDbContext dbContext;
        private IFileRepository repository;

        private DbFile dbFile;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<FileServiceDbContext>()
                                    .UseInMemoryDatabase("InMemoryDatabase")
                                    .Options;
            dbContext = new FileServiceDbContext(dbOptions);
            repository = new FileRepository(dbContext);

            dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
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
        public void ShouldThrowExceptionWhenThereNoFileInDatabaseWithSuchId()
        {
            Assert.Throws<Exception>(() => repository.GetFileById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnFileInfoWhenGettingFileById()
        {
            var result = repository.GetFileById(dbFile.Id);

            var expected = new DbFile
            {
                Id = dbFile.Id,
                Name = dbFile.Name,
                Content = dbFile.Content,
                Extension = dbFile.Extension,
                IsActive = dbFile.IsActive
            };

            SerializerAssert.AreEqual(expected, result);
        }
    }
}