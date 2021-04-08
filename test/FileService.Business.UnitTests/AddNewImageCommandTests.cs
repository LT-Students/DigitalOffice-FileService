﻿using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class AddNewImageCommandTests
    {
        private IAddNewImageCommand command;
        private Mock<IImageRepository> repositoryMock;
        private Mock<IValidator<ImageRequest>> validatorMock;
        private Mock<IImageRequestMapper> mapperMock;

        private ImageRequest imageRequest;

        private bool isBigImage;
        private static DbImage firstDbImage;
        private static DbImage secondDbImage;

        private class TestMapperHelper
        {
            private int calls = 0;

            public DbImage GetImage()
            {
                calls++;

                if (calls == 1)
                {
                    return firstDbImage;
                }
                else if (calls == 2)
                {
                    return secondDbImage;
                }
                else
                {
                    throw new Exception("Too many calls.");
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IImageRepository>();
            validatorMock = new Mock<IValidator<ImageRequest>>();
            mapperMock = new Mock<IImageRequestMapper>();

            var userId = Guid.NewGuid();

            imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_OnePixel",
            };

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);


            firstDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = imageRequest.Content,
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_OnePixel",
                AddedOn = DateTime.UtcNow,
                UserId = userId
            };
            secondDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = imageRequest.Content,
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_OnePixel",
                AddedOn = DateTime.UtcNow,
                UserId = userId
            };

            var mapperHelper = new TestMapperHelper();

            mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, out isBigImage, null))
                .Returns(mapperHelper.GetImage());

            mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out isBigImage, firstDbImage.Id))
                .Returns(mapperHelper.GetImage());

            repositoryMock
                .Setup(x => x.AddNewImage(It.IsAny<DbImage>()))
                .Returns(secondDbImage.Id);

            repositoryMock
                .Setup(x => x.AddNewImage(firstDbImage))
                .Returns(firstDbImage.Id);

            command = new AddNewImageCommand(repositoryMock.Object, validatorMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldAddThumbImmageWhenImageIsBig()
        {
            Assert.AreEqual(firstDbImage.Id, command.Execute(imageRequest));

            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(1));
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, out isBigImage, null), Times.Once);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out isBigImage, firstDbImage.Id), Times.Once);
            //mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), It.IsAny<ImageType>(), out isBigImage, It.IsAny<Guid?>()), Times.Exactly(1));
        }

        [Test]
        public void ShouldNotAddNewImageWhenImageIsNotBig()
        {
            Assert.AreEqual(firstDbImage.Id, command.Execute(imageRequest));

            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(1));
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, out isBigImage, null), Times.Once);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), It.IsAny<ImageType>(), out isBigImage, It.IsAny<Guid?>()), Times.Exactly(1));
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

            Assert.Throws<ValidationException>(() => command.Execute(imageRequest));
            repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Never);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full, out isBigImage, It.IsAny<Guid?>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNullAndImageTypeIsFull()
        {
            mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full, out isBigImage, It.IsAny<Guid?>()))
                 .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(imageRequest));
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNullAndImageTypeIsThumb()
        {
            mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumb, out isBigImage, firstDbImage.Id))
                 .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(imageRequest));
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            repositoryMock
                .Setup(x => x.AddNewImage(firstDbImage))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(imageRequest));
        }
    }
}