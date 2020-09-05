using LT.DigitalOffice.FileService.Database.Entities;
using LT.DigitalOffice.FileService.Mappers;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models;
using LT.DigitalOffice.FileServiceUnitTests.UnitTestLibrary;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileServiceUnitTests.Mappers
{
    public class FileMapperTests
    {
        private IMapper<DbFile, File> dbToDtoMapper;
        private IMapper<FileCreateRequest, DbFile> requestToDbMapper;

        private DbFile dbFile;
        private FileCreateRequest fileRequest;

        [SetUp]
        public void SetUp()
        {
            dbToDtoMapper = new FileMapper();
            requestToDbMapper = new FileMapper();

            fileRequest = new FileCreateRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "DigitalOfficeTestFile"
            };

            dbFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = Convert.FromBase64String("RGlnaXRhbCBPZmA5Y2U="),
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile"
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbMappingObjectIsNull()
        {
            dbFile = null;

            Assert.Throws<ArgumentNullException>(() => dbToDtoMapper.Map(dbFile));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            fileRequest = null;

            Assert.Throws<ArgumentNullException>(() => requestToDbMapper.Map(fileRequest));
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequest()
        {
            var newFile = requestToDbMapper.Map(fileRequest);

            var expectedFile = new DbFile
            {
                Id = newFile.Id,
                Content = Convert.FromBase64String(fileRequest.Content),
                Extension = fileRequest.Extension,
                Name = fileRequest.Name,
                IsActive = true,
                AddedOn = newFile.AddedOn
            };

            SerializerAssert.AreEqual(expectedFile, newFile);
        }

        [Test]
        public void ShouldReturnFileResponseWhenMappingDbFile()
        {
            var newFileDto = dbToDtoMapper.Map(dbFile);

            var expectedFileDto = new File
            {
                Id = newFileDto.Id,
                Content = Convert.ToBase64String(dbFile.Content),
                Extension = dbFile.Extension,
                Name = dbFile.Name
            };

            Assert.IsInstanceOf<Guid>(newFileDto.Id);
            SerializerAssert.AreEqual(expectedFileDto, newFileDto);
        }
    }
}