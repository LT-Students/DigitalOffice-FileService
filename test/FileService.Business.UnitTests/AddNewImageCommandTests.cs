using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class AddNewImageCommandTests
    {
        private IAddNewImageCommand command;
        private Mock<IHttpContextAccessor> httpContextAccessor;
        private Mock<IImageRepository> repositoryMock;
        private Mock<IValidator<ImageRequest>> validatorMock;
        private Mock<IImageRequestMapper> mapperMock;

        private ImageRequest imageRequest;

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
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            repositoryMock = new Mock<IImageRepository>();
            validatorMock = new Mock<IValidator<ImageRequest>>();
            mapperMock = new Mock<IImageRequestMapper>();

            var userId = Guid.NewGuid();

            httpContextAccessor
                .Setup(x => x.HttpContext.GetUserId())
                .Returns(userId);

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
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_OnePixel",
                AddedOn = DateTime.UtcNow,
                UserId = userId
            };
            secondDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_OnePixel",
                AddedOn = DateTime.UtcNow,
                UserId = userId
            };

            var mapperHelper = new TestMapperHelper();

            mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full))
                .Returns(mapperHelper.GetImage());

            repositoryMock
                .Setup(x => x.AddNewImage(It.IsAny<DbImage>()))
                .Returns(secondDbImage.Id);

            repositoryMock
                .Setup(x => x.AddNewImage(firstDbImage))
                .Returns(firstDbImage.Id);

            command = new AddNewImageCommand(httpContextAccessor.Object, repositoryMock.Object, validatorMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldAddNewImageAndThumbImmage()
        {
            Assert.AreEqual(firstDbImage.Id, command.Execute(imageRequest));

            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full), Times.Once);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Thumbs), Times.Once);
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
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>(), ImageType.Full), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNull1()
        {
            mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Full))
                 .Throws(new NullReferenceException());

            Assert.Throws<NullReferenceException>(() => command.Execute(imageRequest));
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNull2()
        {
            mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>(), ImageType.Thumbs))
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