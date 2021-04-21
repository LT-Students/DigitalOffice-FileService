using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class AddNewFileRepositoryTests
    {
        private IFileRepository _repository;
        private FileServiceDbContext _dbContext;

        private DbFile _newFile;

        [SetUp]
        public void SetUp()
        {
            var dbOptionsFileService = new DbContextOptionsBuilder<FileServiceDbContext>()
                .UseInMemoryDatabase("FileServiceTestDatabase")
                .Options;

            _dbContext = new FileServiceDbContext(dbOptionsFileService);
            _repository = new FileRepository(_dbContext);

            _newFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };
        }

        [Test]
        public void ShouldAddNewFileToDatabase()
        {
            Assert.AreEqual(_newFile.Id, _repository.AddNewFile(_newFile));
            Assert.That(_dbContext.Files.Find(_newFile.Id), Is.EqualTo(_newFile));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingFileWithRepeatingId()
        {
            _repository.AddNewFile(_newFile);

            Assert.Throws<ArgumentException>(() => _repository.AddNewFile(_newFile));
            Assert.That(_dbContext.Files.Find(_newFile.Id), Is.EqualTo(_newFile));
        }

        [TearDown]
        public void CleanInMemoryDatabase()
        {
            if (_dbContext.Database.IsInMemory())
            {
                _dbContext.Database.EnsureDeleted();
            }
        }
    }
}
