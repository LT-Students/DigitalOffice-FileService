using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class GetFileByIdCommandTests
    {
        private IGetFileByIdCommand command;
        private Mock<IFileRepository> repositoryMock;
        private Mock<IFileMapper> mapperMock;

        private DbFile file;
        private Guid fileId;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IFileRepository>();
            mapperMock = new Mock<IFileMapper>();
            command = new GetFileByIdCommand(repositoryMock.Object, mapperMock.Object);

            fileId = Guid.NewGuid();
            file = new DbFile
            {
                Id = fileId,
                Name = "File",
                Content = Convert.FromBase64String("RGlnaXRhbCBPZmA5Y2U="),
                Extension = ".jpg"
            };
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.GetFileById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(fileId));
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsIt()
        {
            mapperMock.Setup(x => x.Map(It.IsAny<DbFile>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(fileId));
        }

        [Test]
        public void ShouldReturnFileInfo()
        {
            var expected = new File
            {
                Id = file.Id,
                Name = file.Name,
                Content = Convert.ToBase64String(file.Content),
                Extension = file.Extension
            };

            repositoryMock
                .Setup(x => x.GetFileById(It.IsAny<Guid>()))
                .Returns(file);
            mapperMock
                .Setup(x => x.Map(It.IsAny<DbFile>()))
                .Returns(expected);

            var result = command.Execute(fileId);

            SerializerAssert.AreEqual(expected, result);
        }
    }
}