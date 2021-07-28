using LT.DigitalOffice.FileService.Mappers.Models;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Models
{
    public class ImageInfoMapperTests
    {
        private IImageInfoMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new ImageInfoMapper();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbImageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbImage dbImage = new()
            {
                Id = Guid.NewGuid(),
                Name = "MyImage",
                ParentId = Guid.NewGuid(),
                Content = "Content",
                Extension = ".jpg",
                ImageType = (int)ImageType.Thumb,
                UserId = Guid.NewGuid(),
                AddedOn = DateTime.UtcNow,
                IsActive = true
            };

            ImageInfo imageInfo = new()
            {
                Id = dbImage.Id,
                ParentId = dbImage.ParentId,
                Content = dbImage.Content,
                Extension = dbImage.Extension,
                Name = dbImage.Name,
                Type = ImageType.Thumb
            };

            SerializerAssert.AreEqual(imageInfo, _mapper.Map(dbImage));
        }
    }
}
