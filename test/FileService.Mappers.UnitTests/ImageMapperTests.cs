using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers;
using LT.DigitalOffice.FileService.Models.Db;
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
            requestToDbMapper = new ImageMapper();

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
            var newFile = requestToDbMapper.Map(imageRequest);

            var expectedFile = new DbFile
            {
                Id = newFile.Id,
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = imageRequest.Extension,
                Name = imageRequest.Name,
                IsActive = true,
                AddedOn = newFile.AddedOn
            };

            SerializerAssert.AreEqual(expectedFile, newFile);
        }
    }
}