using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class AddNewFileCommandTests
    {
        private IAddNewFileCommand command;
        private Mock<IFileRepository> repositoryMock;
        private Mock<IValidator<File>> validatorMock;
        private Mock<IFileMapper> mapperMock;

        private DbFile newFile;
        private File fileRequest;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IFileRepository>();
            validatorMock = new Mock<IValidator<File>>();
            mapperMock = new Mock<IFileMapper>();

            newFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile",
                AddedOn = DateTime.UtcNow
            };

            fileRequest = new File
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "DigitalOfficeTestFile"
            };

            mapperMock
                .Setup(f => f.Map(It.IsAny<File>()))
                .Returns(newFile);
            command = new AddNewFileCommand(repositoryMock.Object, validatorMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldAddNewFile()
        {
            var fileId = Guid.NewGuid();

            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            repositoryMock
                .Setup(x => x.AddNewFile(It.IsAny<DbFile>()))
                .Returns(fileId);

            Assert.AreEqual(fileId, command.Execute(fileRequest));
            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewFile(newFile), Times.Once);
            mapperMock.Verify(m => m.Map(fileRequest), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowsException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => command.Execute(fileRequest));
            repositoryMock.Verify(r => r.AddNewFile(newFile), Times.Never);
            mapperMock.Verify(m => m.Map(fileRequest), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            repositoryMock
                .Setup(x => x.AddNewFile(It.IsAny<DbFile>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(fileRequest), "GUID duplicated error");
            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewFile(newFile), Times.Once);
            mapperMock.Verify(m => m.Map(fileRequest), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenFileRequestIsNull()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                 .Setup(x => x.Map(It.IsAny<File>()))
                 .Throws(new NullReferenceException());

            fileRequest = null;

            Assert.Throws<NullReferenceException>(() => command.Execute(fileRequest), "Request is null");
        }

        public void ShouldThrowNullReferenceExceptionWhenMapperThrowsIt()
        {
            mapperMock
                .Setup(x => x.Map(It.IsAny<File>()))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => command.Execute(fileRequest));
        }
    }
}