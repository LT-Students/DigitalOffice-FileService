using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class DeleteFileByIdCommandTests
    {
        private IDeleteFileByIdCommand command;
        private Mock<IFileRepository> repositoryMock;

        private Guid fileId;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IFileRepository>();
            command = new DeleteFileByIdCommand(repositoryMock.Object);

            fileId = Guid.NewGuid();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.DeleteFileById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(fileId));
        }

        [Test]
        public void ShouldDeleteFile()
        {
            repositoryMock
                .Setup(x => x.DeleteFileById(It.IsAny<Guid>()));

            Assert.DoesNotThrow(() => command.Execute(fileId));
            repositoryMock.Verify(repository => repository.DeleteFileById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
