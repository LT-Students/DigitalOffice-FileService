using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class AddNewImageRepositoryTests
    {
        private IImageRepository _repository;
        private FileServiceDbContext _dbContext;

        private DbImage _newImage;

        [SetUp]
        public void SetUp()
        {
            var dbOptionsFileService = new DbContextOptionsBuilder<FileServiceDbContext>()
                .UseInMemoryDatabase("FileServiceTestDatabase")
                .Options;

            _dbContext = new FileServiceDbContext(dbOptionsFileService);
            _repository = new ImageRepository(_dbContext);

            _newImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_Photo"
            };
        }

        [Test]
        public void ShouldAddNewImageToDatabase()
        {
            Assert.AreEqual(_newImage.Id, _repository.AddNewImage(_newImage));
            Assert.That(_dbContext.Images.Find(_newImage.Id), Is.EqualTo(_newImage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingImageWithRepeatingId()
        {
            _repository.AddNewImage(_newImage);

            Assert.Throws<ArgumentException>(() => _repository.AddNewImage(_newImage));
            Assert.That(_dbContext.Images.Find(_newImage.Id), Is.EqualTo(_newImage));
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
