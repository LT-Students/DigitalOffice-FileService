using FluentValidation;
using LT.DigitalOffice.FileService.Business.Commands.Image;
using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
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
        private Mock<IImageRequestValidator> _validatorMock;
        private Mock<IDbImageMapper> _mapperMock;
        private Mock<IHttpContextAccessor> _accessorMock;

        private ImageRequest _imageRequest;

        private bool _isBigImage;
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
            _isBigImage = false;

            _repositoryMock = new Mock<IImageRepository>();
            _repositoryMock
                .Setup(x => x.AddNewImage(_thumbDbImage))
                .Returns(_thumbDbImage.Id);
            _repositoryMock
                .Setup(x => x.AddNewImage(_fullDbImage))
                .Returns(_fullDbImage.Id);

            _validatorMock = new Mock<IImageRequestValidator>();
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _mapperMock = new Mock<IDbImageMapper>();
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_fullDbImage);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_thumbDbImage);

            ClientRequestUp();

            _command = new AddNewImageCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessorMock.Object);
        }

        [Test]
        public void ShouldAddOnlyFullAndThumbImage()
        {
           _isBigImage = true;
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_fullDbImage);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_thumbDbImage);

            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Full,
                    out _isBigImage,
                    _userId,
                    null),
                Times.Once);
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Thumb,
                    out _isBigImage,
                    _userId,
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public void ShouldAddOnlyFullImage()
        {
            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Once);
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Full,
                    out _isBigImage,
                    _userId,
                    null),
                Times.Once);
        }

        [Test]
        public void ShouldAddOnlyFullAndThumbImageFromBrokerRequest()
        {
            _isBigImage = true;
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_fullDbImage);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .Returns(_thumbDbImage);

            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest, _userId));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Full,
                    out _isBigImage,
                    _userId,
                    null),
                Times.Once);
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Thumb,
                    out _isBigImage,
                    _userId,
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public void ShouldAddOnlyFullImageFromBrokerRequest()
        {
            Assert.AreEqual(_fullDbImage.Id, _command.Execute(_imageRequest, _userId));

            _validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Once);
            _mapperMock.Verify(
                m => m.Map(
                    It.IsAny<ImageRequest>(),
                    ImageType.Full,
                    out _isBigImage,
                    _userId,
                    null),
                Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorThrowsException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_imageRequest));
            _repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Never);
            _mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, out _isBigImage, It.IsAny<Guid>(), It.IsAny<Guid?>()), Times.Never);
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