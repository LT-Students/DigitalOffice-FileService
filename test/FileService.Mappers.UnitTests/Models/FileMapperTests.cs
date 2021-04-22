using LT.DigitalOffice.FileService.Mappers.Db;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Models
{
    public class FileMapperTests
    {
        private IFileInfoMapper _dbToDtoMapper;
        private IDbFileMapper _requestToDbMapper;

        private DbFile _dbFile;
        private FileInfo _fileRequest;

        [SetUp]
        public void SetUp()
        {
            _dbToDtoMapper = new FileInfoMapper();
            _requestToDbMapper = new DbFileMapper();

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

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbMappingObjectIsNull()
        {
            _dbFile = null;

            Assert.Throws<ArgumentNullException>(() => _dbToDtoMapper.Map(_dbFile));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            _fileRequest = null;

            Assert.Throws<ArgumentNullException>(() => _requestToDbMapper.Map(_fileRequest));
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newFile = _requestToDbMapper.Map(_fileRequest);

            var expectedFile = new DbFile
            {
                Id = newFile.Id,
                Content = _fileRequest.Content,
                Extension = _fileRequest.Extension,
                Name = _fileRequest.Name,
                IsActive = true,
                AddedOn = newFile.AddedOn
            };

            SerializerAssert.AreEqual(expectedFile, newFile);
        }

        [Test]
        public void ShouldReturnFileResponseWhenMappingDbFile()
        {
            var newFileDto = _dbToDtoMapper.Map(_dbFile);

            var expectedFileDto = new FileInfo
            {
                Id = newFileDto.Id,
                Content = _dbFile.Content,
                Extension = _dbFile.Extension,
                Name = _dbFile.Name
            };

            Assert.IsInstanceOf<Guid>(newFileDto.Id);
            SerializerAssert.AreEqual(expectedFileDto, newFileDto);
        }
    }
}