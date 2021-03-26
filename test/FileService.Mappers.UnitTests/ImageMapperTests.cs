using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests
{
    public class ImageMapperTests
    {
        private IImageRequestMapper requestToDbMapper;
        private Mock<IImageResizeAlgorithm> algorithmMock;

        private ImageRequest imageRequest;
        private byte[] resizedImageContent;

        [SetUp]
        public void SetUp()
        {
            algorithmMock = new Mock<IImageResizeAlgorithm>();
            requestToDbMapper = new ImageRequestMapper(algorithmMock.Object);

            imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_Photo",
                UserId = Guid.NewGuid()
            };

            resizedImageContent = new byte[] { 0, 1, 1, 0 };

            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Returns(resizedImageContent);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            imageRequest = null;

            Assert.Throws<ArgumentNullException>(() => requestToDbMapper.Map(imageRequest, ImageType.Full));
            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newImage = requestToDbMapper.Map(imageRequest, ImageType.Full);

            var expectedImage = new DbImage
            {
                Id = newImage.Id,
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = imageRequest.Extension,
                Name = imageRequest.Name,
                UserId = imageRequest.UserId,
                ImageType = (int)ImageType.Full,
                AddedOn = newImage.AddedOn,
                IsActive = true
            };

            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Once);


            SerializerAssert.AreEqual(expectedImage, newImage);
        }

        [Test]
        public void ShouldThrowExceptionWhenAlgorithmThrowsExceptionAndImageTypeIsThumbs()
        {
            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => requestToDbMapper.Map(imageRequest, ImageType.Thumbs));
        }

        [Test]
        public void ShouldNotThrowExceptionWhenAlgorithmThrowsExceptionAndImageTypeIsFull()
        {
            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => requestToDbMapper.Map(imageRequest, ImageType.Thumbs));
        }

        // Add tests
    }
}