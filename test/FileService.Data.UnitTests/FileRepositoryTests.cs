using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class FileRepositoryTests
    {
        private IFileRepository _repository;
        private FileServiceDbContext _dbContext;

        private DbFile _dbFile;

        [SetUp]
        public void SetUp()
        {
            var dbOptionsFileService = new DbContextOptionsBuilder<FileServiceDbContext>()
                .UseInMemoryDatabase("FileServiceTestDatabase")
                .Options;

            _dbContext = new FileServiceDbContext(dbOptionsFileService);
            _repository = new FileRepository(_dbContext);

            _dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };
        }

        #region AddFile

        [Test]
        public void ShouldAddNewFileToDatabase()
        {
            Assert.AreEqual(_dbFile.Id, _repository.AddFile(_dbFile));
            Assert.That(_dbContext.Files.Find(_dbFile.Id), Is.EqualTo(_dbFile));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingFileWithRepeatingId()
        {
            _repository.AddFile(_dbFile);

            Assert.Throws<ArgumentException>(() => _repository.AddFile(_dbFile));
            Assert.That(_dbContext.Files.Find(_dbFile.Id), Is.EqualTo(_dbFile));
        }

        #endregion

        #region DisableFile

        [Test]
        public void ShouldThrowExceptionWhenFileWasNotFound()
        {
            _dbContext.Files.Add(_dbFile);

            Assert.Throws<NotFoundException>(() => _repository.DisableFile(Guid.NewGuid()));
        }

        [Test]
        public void ShouldDisableFile()
        {
            _dbContext.Files.Add(_dbFile);

            _repository.DisableFile(_dbFile.Id);

            Assert.That(_dbContext.Files.Find(_dbFile.Id).IsActive == false);
        }

        #endregion

        #region GetFile

        [Test]
        public void ShouldThrowExceptionWhenThereNoFileInDatabaseWithSuchId()
        {
            Assert.Throws<NotFoundException>(() => _repository.GetFile(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnFileInfoWhenGettingFileById()
        {
            var result = _repository.GetFile(_dbFile.Id);

            var expected = new DbFile
            {
                Id = _dbFile.Id,
                Name = _dbFile.Name,
                Content = _dbFile.Content,
                Extension = _dbFile.Extension,
                IsActive = _dbFile.IsActive
            };

            SerializerAssert.AreEqual(expected, result);
        }

        #endregion

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
