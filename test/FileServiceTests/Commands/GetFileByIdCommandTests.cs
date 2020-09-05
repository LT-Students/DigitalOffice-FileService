using LT.DigitalOffice.FileService.Commands;
using LT.DigitalOffice.FileService.Commands.Interfaces;
using LT.DigitalOffice.FileService.Database.Entities;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models;
using LT.DigitalOffice.FileService.Repositories.Interfaces;
using LT.DigitalOffice.FileServiceUnitTests.UnitTestLibrary;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileServiceUnitTests.Commands
{
    public class GetFileByIdCommandTests
    {
        private IGetFileByIdCommand command;
        private Mock<IFileRepository> repositoryMock;
        private Mock<IMapper<DbFile, File>> mapperMock;

        private DbFile file;
        private Guid fileId;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IFileRepository>();
            mapperMock = new Mock<IMapper<DbFile, File>>();
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