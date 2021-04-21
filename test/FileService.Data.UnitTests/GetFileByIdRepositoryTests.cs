using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class GetFileByIdRepositoryTests
    {
        private FileServiceDbContext _dbContext;
        private IFileRepository _repository;

        private DbFile _dbFile;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<FileServiceDbContext>()
                                    .UseInMemoryDatabase("InMemoryDatabase")
                                    .Options;
            _dbContext = new FileServiceDbContext(dbOptions);
            _repository = new FileRepository(_dbContext);

            _dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };

            _dbContext.Files.Add(_dbFile);
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void Clean()
        {
            if (_dbContext.Database.IsInMemory())
            {
                _dbContext.Database.EnsureDeleted();
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenThereNoFileInDatabaseWithSuchId()
        {
            Assert.Throws<NotFoundException>(() => _repository.GetFileById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnFileInfoWhenGettingFileById()
        {
            var result = _repository.GetFileById(_dbFile.Id);

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
    }
}