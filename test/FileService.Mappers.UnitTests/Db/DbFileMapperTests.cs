using LT.DigitalOffice.FileService.Mappers.Db;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Db
{
    public class DbFileMapperTests
    {
        private IDbFileMapper _dbFileMapper;

        private DbFile _dbFile;
        private AddFileRequest _fileRequest;

        [SetUp]
        public void SetUp()
        {
            _dbFileMapper = new DbFileMapper();

            _fileRequest = new AddFileRequest
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
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            _fileRequest = null;

            Assert.Throws<ArgumentNullException>(() => _dbFileMapper.Map(_fileRequest));
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newFile = _dbFileMapper.Map(_fileRequest);

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
    }
}