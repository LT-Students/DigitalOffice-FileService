using LT.DigitalOffice.FileService.Business.Commands.File;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Business.UnitTests.Commands.File
{
    public class DisableFileCommandTests
    {
        private IDisableFileCommand _command;
        private Mock<IFileRepository> _repositoryMock;

        private Guid fileId;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IFileRepository>();
            _command = new DisableFileCommand(_repositoryMock.Object);

            fileId = Guid.NewGuid();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.DisableFileById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(fileId));
        }

        [Test]
        public void ShouldDisableFile()
        {
            _repositoryMock
                .Setup(x => x.DisableFileById(It.IsAny<Guid>()));

            Assert.DoesNotThrow(() => _command.Execute(fileId));
            _repositoryMock.Verify(repository => repository.DisableFileById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
