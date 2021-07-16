using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Commands.File;
using LT.DigitalOffice.FileService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.UnitTests.Commands.File
{
    public class AddFileCommandTests
    {
        private IAddFileCommand _command;
        private Mock<IFileRepository> _repositoryMock;
        private Mock<IFileRequestValidator> _validatorMock;
        private Mock<IDbFileMapper> _mapperMock;

        private DbFile _newFile;
        private FileRequest _fileRequest;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IFileRepository>();
            _validatorMock = new Mock<IFileRequestValidator>();
            _mapperMock = new Mock<IDbFileMapper>();

            _newFile = new DbFile
            {
                Id = Guid.NewGuid(),
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                IsActive = true,
                Name = "DigitalOfficeTestFile",
                AddedOn = DateTime.UtcNow
            };

            _fileRequest = new FileRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "DigitalOfficeTestFile"
            };

            _mapperMock
                .Setup(f => f.Map(It.IsAny<FileRequest>()))
                .Returns(_newFile);
            _command = new AddFileCommand(_repositoryMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [Test]
        public void ShouldAddNewFile()
        {
            var fileId = Guid.NewGuid();

            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _repositoryMock
                .Setup(x => x.AddFile(It.IsAny<DbFile>()))
                .Returns(fileId);

            Assert.AreEqual(fileId, _command.Execute(_fileRequest));
            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddFile(_newFile), Times.Once);
            _mapperMock.Verify(m => m.Map(_fileRequest), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowsException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => _command.Execute(_fileRequest));
            _repositoryMock.Verify(r => r.AddFile(_newFile), Times.Never);
            _mapperMock.Verify(m => m.Map(_fileRequest), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _repositoryMock
                .Setup(x => x.AddFile(It.IsAny<DbFile>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_fileRequest), "GUID duplicated error");
            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddFile(_newFile), Times.Once);
            _mapperMock.Verify(m => m.Map(_fileRequest), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenFileRequestIsNull()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                 .Setup(x => x.Map(It.IsAny<FileRequest>()))
                 .Throws(new NullReferenceException());

            _fileRequest = null;

            Assert.Throws<NullReferenceException>(() => _command.Execute(_fileRequest), "Request is null");
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenMapperThrowsIt()
        {
            _mapperMock
                .Setup(x => x.Map(It.IsAny<FileRequest>()))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_fileRequest));
        }
    }
}