using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    class DisableFileByIdRepositoryTests
    {
        private FileServiceDbContext _dbContext;
        private IFileRepository _repository;
        private Guid _fileId;
        private DbFile _dbFile;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<FileServiceDbContext>()
                                    .UseInMemoryDatabase("InMemoryDatabase")
                                    .Options;
            _dbContext = new FileServiceDbContext(dbOptions);
            _repository = new FileRepository(_dbContext);

            _fileId = Guid.NewGuid();
            _dbFile = new DbFile
            {
                Id = _fileId,
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
        public void ShouldThrowExceptionWhenFileWasNotFound()
        {
            Assert.Throws<NotFoundException>(() => _repository.DisableFileById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldDisableFile()
        {
            _repository.DisableFileById(_dbFile.Id);

            Assert.That(_dbContext.Files.Find(_fileId).IsActive == false);
        }
    }
}
