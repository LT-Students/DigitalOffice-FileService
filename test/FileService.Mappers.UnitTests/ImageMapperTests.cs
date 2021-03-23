using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers;
using LT.DigitalOffice.FileService.Mappers.RequestMappers;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests
{
    public class ImageMapperTests
    {
        private IMapper<ImageRequest, DbImage> requestToDbMapper;

        private ImageRequest imageRequest;

        [SetUp]
        public void SetUp()
        {
            requestToDbMapper = new ImageRequestMapper();

            imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_Photo"
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            imageRequest = null;

            Assert.Throws<ArgumentNullException>(() => requestToDbMapper.Map(imageRequest));
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newImage = requestToDbMapper.Map(imageRequest);

            var expectedImage = new DbImage
            {
                Id = newImage.Id,
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = imageRequest.Extension,
                Name = imageRequest.Name,
                IsActive = true,
                ImageType = (int)ImageType.Full,
                AddedOn = newImage.AddedOn
            };

            SerializerAssert.AreEqual(expectedImage, newImage);
        }
    }
}