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
        private IImageRepository repository;
        private FileServiceDbContext dbContext;

        private DbImage newImage;

        [SetUp]
        public void SetUp()
        {
            var dbOptionsFileService = new DbContextOptionsBuilder<FileServiceDbContext>()
                .UseInMemoryDatabase("FileServiceTestDatabase")
                .Options;

            dbContext = new FileServiceDbContext(dbOptionsFileService);
            repository = new ImageRepository(dbContext);

            newImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = Convert.FromBase64String("RGlnaXRhbCBPZmA5Y2U="),
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_Photo"
            };
        }

        [Test]
        public void ShouldAddNewImageToDatabase()
        {
            Assert.AreEqual(newImage.Id, repository.AddNewImage(newImage));
            Assert.That(dbContext.Files.Find(newImage.Id), Is.EqualTo(newImage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAddingImageWithRepeatingId()
        {
            repository.AddNewImage(newImage);

            Assert.Throws<ArgumentException>(() => repository.AddNewImage(newImage));
            Assert.That(dbContext.Files.Find(newImage.Id), Is.EqualTo(newImage));
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
