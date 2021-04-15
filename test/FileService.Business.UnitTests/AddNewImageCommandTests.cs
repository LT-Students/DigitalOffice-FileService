using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class AddNewImageCommandTests
    {
        private IAddNewImageCommand _command;
        private Mock<IImageRepository> _repositoryMock;
        private Mock<IValidator<ImageRequest>> _validatorMock;
        private Mock<IDbImageMapper> _mapperMock;
        private Mock<IHttpContextAccessor> _accessorMock;

        private ImageRequest _imageRequest;

        private DbImage _fullDbImage;
        private DbImage _thumbDbImage;

        private Guid _userId;
        private const string _name = "Name";
        private const string _extension = "ext";
        private const string _content = "content";

        private void ClientRequestUp()
        {
            IDictionary<object, object> httpContextItems = new Dictionary<object, object>();

            httpContextItems.Add("UserId", _userId);

            _accessorMock = new Mock<IHttpContextAccessor>();
            _accessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(httpContextItems);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid();

            _imageRequest = new ImageRequest
            {
                Name = _name,
                Extension = _extension,
                Content = _content
            };

            _fullDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = _imageRequest.Content,
                Extension = _imageRequest.Extension,
                ImageType = (int)ImageType.Full,
                IsActive = true,
                Name = _imageRequest.Name,
                AddedOn = DateTime.UtcNow,
                UserId = _userId,
                ParentId = null
            };

            _thumbDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = _imageRequest.Content,
                Extension = _imageRequest.Extension,
                ImageType = (int)ImageType.Thumb,
                IsActive = true,
                Name = _imageRequest.Name,
                AddedOn = DateTime.UtcNow,
                UserId = _userId,
                ParentId = _fullDbImage.Id
            };
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IImageRepository>();
            _repositoryMock
                .Setup(x => x.AddNewImage(_thumbDbImage))
                .Returns(_thumbDbImage.Id);
            _repositoryMock
                .Setup(x => x.AddNewImage(_fullDbImage))
                .Returns(_fullDbImage.Id);

            _validatorMock = new Mock<IValidator<ImageRequest>>();
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _mapperMock = new Mock<IDbImageMapper>();
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_fullDbImage);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_thumbDbImage);

            ClientRequestUp();

            _command = new AddNewImageCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessorMock.Object);
        }

        [Test]
        public void ShouldAddNewImageAndThumbImmageByController()
        {
            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, _userId, null), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, _userId, _fullDbImage.Id), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), It.IsAny<ImageType>(), It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Exactly(2));
        }

        [Test]
        public void ShouldAddNewImageAndThumbImmageByBrokerRequest()
        {
            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest, _userId));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, _userId, null), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, _userId, _fullDbImage.Id), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), It.IsAny<ImageType>(), It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Exactly(2));
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

            Assert.Throws<ValidationException>(() => _command.Execute(_imageRequest));
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Never);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNullAndImageTypeIsFull()
        {
            _mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                 .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => _command.Execute(_imageRequest));
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNullAndImageTypeIsThumb()
        {
            _mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                 .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => _command.Execute(_imageRequest));
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _repositoryMock
                .Setup(x => x.AddNewImage(_fullDbImage))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_imageRequest));
        }
    }
}