using LT.DigitalOffice.FileService.Mappers.Db;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Db
{
    public class DbFileMapperTests
    {
        private AutoMocker _autoMocker;
        private IDbFileMapper _dbFileMapper;
        private IDictionary<object, object> _items;
        private DbFile _dbFile;
        private AddFileRequest _fileRequest;
        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new();

            _userId = Guid.NewGuid();
            _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _dbFileMapper = _autoMocker.CreateInstance<DbFileMapper>();

            _autoMocker
               .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
               .Returns(_items);

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

        /*[Test]
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
                CreatedAtUtc = newFile.CreatedAtUtc,
                CreatedBy = _userId
            };

            SerializerAssert.AreEqual(expectedFile, newFile);
        }*/
    }
}