using LT.DigitalOffice.FileService.Mappers.Models;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Models
{
    public class FileInfoMapperTests
    {
        private IFileDataMapper _fileInfoMapper;

        private DbFile _dbFile;
        private FileInfo _fileRequest;

        [SetUp]
        public void SetUp()
        {
            _fileInfoMapper = new FileDataMapper();

            _fileRequest = new FileInfo
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "DigitalOfficeTestFile"
            };

            _dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };
        }

        /*[Test]
        public void ShouldThrowArgumentNullExceptionWhenDbMappingObjectIsNull()
        {
            _dbFile = null;

            Assert.Throws<ArgumentNullException>(() => _fileInfoMapper.Map(_dbFile));
        }

        [Test]
        public void ShouldReturnFileResponseWhenMappingDbFile()
        {
            var newFileDto = _fileInfoMapper.Map(_dbFile);

            var expectedFileDto = new FileInfo
            {
                Id = newFileDto.Id,
                Content = _dbFile.Content,
                Extension = _dbFile.Extension,
                Name = _dbFile.Name
            };

            Assert.IsInstanceOf<Guid>(newFileDto.Id);
            SerializerAssert.AreEqual(expectedFileDto, newFileDto);
        }*/
    }
}