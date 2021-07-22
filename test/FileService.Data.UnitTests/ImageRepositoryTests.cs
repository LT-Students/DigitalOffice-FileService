using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Data.UnitTests
{
    public class ImageRepositoryTests
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

        [TearDown]
        public void CleanInMemoryDatabase()
        {
            if (_dbContext.Database.IsInMemory())
            {
                _dbContext.Database.EnsureDeleted();
            }
        }

        #region AddImage

        [Test]
        public void ShouldAddImageToDatabase()
        {
            Assert.AreEqual(_newImage.Id, _repository.Add(_newImage));
            Assert.That(_dbContext.Images.Find(_newImage.Id), Is.EqualTo(_newImage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingImageWithRepeatingId()
        {
            _repository.Add(_newImage);

            Assert.Throws<ArgumentException>(() => _repository.Add(_newImage));
            Assert.That(_dbContext.Images.Find(_newImage.Id), Is.EqualTo(_newImage));
        }

        #endregion

        #region Get Tests

        [Test]
        public void ShouldThrowNotFoundExceptionWhenImageDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get(Guid.NewGuid()));
        }

        [Test]
        public void ShouldGetImageSuccessfuly()
        {
            DbImage image = new()
            {
                Id = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                Name = "name",
                Content = "content",
                Extension = "extansion",
                AddedOn = DateTime.UtcNow,
                IsActive = true,
                ImageType = 0,
                UserId = Guid.NewGuid()
            };

            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            SerializerAssert.AreEqual(image, _repository.Get(image.Id));
        }

        [Test]
        public void ShouldGetImagesByIdsSuccessfuly()
        {
            DbImage image1 = new()
            {
                Id = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                Name = "name",
                Content = "content",
                Extension = "extansion",
                AddedOn = DateTime.UtcNow,
                IsActive = true,
                ImageType = 0,
                UserId = Guid.NewGuid()
            };

            DbImage image2 = new()
            {
                Id = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                Name = "name",
                Content = "content",
                Extension = "extansion",
                AddedOn = DateTime.UtcNow,
                IsActive = true,
                ImageType = 0,
                UserId = Guid.NewGuid()
            };

            _dbContext.Images.Add(image1);
            _dbContext.Images.Add(image2);
            _dbContext.SaveChanges();

            List<DbImage> images = _repository.Get(new List<Guid> { image1.Id, image2.Id });

            Assert.IsTrue(images.Contains(image1));
            Assert.IsTrue(images.Contains(image2));
            Assert.IsTrue(images.Count == 2);
        }

        #endregion
    }
}
