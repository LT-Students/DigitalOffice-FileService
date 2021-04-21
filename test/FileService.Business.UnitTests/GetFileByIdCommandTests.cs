using LT.DigitalOffice.FileService.Business.Commands.File;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class GetFileByIdCommandTests
    {
        private IGetFileByIdCommand _command;
        private Mock<IFileRepository> _repositoryMock;
        private Mock<IFileInfoMapper> _mapperMock;

        private DbFile _file;
        private Guid _fileId;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IFileRepository>();
            _mapperMock = new Mock<IFileInfoMapper>();
            _command = new GetFileByIdCommand(_repositoryMock.Object, _mapperMock.Object);

            _fileId = Guid.NewGuid();
            _file = new DbFile
            {
                Id = _fileId,
                Name = "File",
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".jpg"
            };
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.GetFileById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_fileId));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            _mapperMock.Setup(x => x.Map(It.IsAny<DbFile>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_fileId));
        }

        [Test]
        public void ShouldReturnFileInfo()
        {
            var expected = new FileInfo
            {
                Id = _file.Id,
                Name = _file.Name,
                Content = _file.Content,
                Extension = _file.Extension
            };

            _repositoryMock
                .Setup(x => x.GetFileById(It.IsAny<Guid>()))
                .Returns(_file);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<DbFile>()))
                .Returns(expected);

            var result = _command.Execute(_fileId);

            SerializerAssert.AreEqual(expected, result);
        }
    }
}