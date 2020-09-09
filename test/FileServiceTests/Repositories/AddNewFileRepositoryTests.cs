using LT.DigitalOffice.FileService.Data;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileServiceUnitTests.Repositories
{
    public class AddNewFileRepositoryTests
    {
        private IFileRepository repository;
        private FileServiceDbContext dbContext;

        private DbFile newFile;

        [SetUp]
        public void SetUp()
        {
            var dbOptionsFileService = new DbContextOptionsBuilder<FileServiceDbContext>()
                .UseInMemoryDatabase("FileServiceTestDatabase")
                .Options;

            dbContext = new FileServiceDbContext(dbOptionsFileService);
            repository = new FileRepository(dbContext);

            newFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = Convert.FromBase64String("RGlnaXRhbCBPZmA5Y2U="),
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };
        }

        [Test]
        public void ShouldAddNewFileToDatabase()
        {
            Assert.AreEqual(newFile.Id, repository.AddNewFile(newFile));
            Assert.That(dbContext.Files.Find(newFile.Id), Is.EqualTo(newFile));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingFileWithRepeatingId()
        {
            repository.AddNewFile(newFile);

            Assert.Throws<ArgumentException>(() => repository.AddNewFile(newFile));
            Assert.That(dbContext.Files.Find(newFile.Id), Is.EqualTo(newFile));
        }

        [TearDown]
        public void CleanInMemoryDatabase()
        {
            if (dbContext.Database.IsInMemory())
            {
                dbContext.Database.EnsureDeleted();
            }
        }
    }
}
