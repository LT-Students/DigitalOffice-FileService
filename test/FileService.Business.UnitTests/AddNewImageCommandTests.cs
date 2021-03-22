using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static LT.DigitalOffice.FileService.Business.AddNewImageCommand;

namespace LT.DigitalOffice.FileService.Business.UnitTests
{
    public class AddNewImageCommandTests
    {
        private IAddNewImageCommand command;
        private Mock<IImageRepository> repositoryMock;
        private Mock<IValidator<ImageRequest>> validatorMock;
        private Mock<IImageMapper> mapperMock;
        private Mock<IImageResizeAlgorithm> algorithmMock;

        private ImageRequest imageRequest;

        private static DbImage firstDbImage;
        private static DbImage secondDbImage;
        private byte[] resizedImageContent;

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
            mapperMock = new Mock<IImageMapper>();
            algorithmMock = new Mock<IImageResizeAlgorithm>();

            imageRequest = new ImageRequest
            {
                Content = "data:image/png;base64," +
                    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAHGVYSWZJSSoACAAAAAEAE" +
                    "gEDAAEAAAABAAAAGgAAAAAAL74gKgAAAA1JREFUeJxjefLn/38ACZoD485K0AgAAAAASUVORK5CYII=",
                Extension = ".png",
                Name = "Spartak_OnePixel"
            };

            resizedImageContent = new byte[] { 0, 1, 1, 0 };

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
                AddedOn = DateTime.UtcNow
            };
            secondDbImage = new DbImage
            {
                Id = Guid.NewGuid(),
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = ".png",
                IsActive = true,
                Name = "Spartak_OnePixel",
                AddedOn = DateTime.UtcNow
            };

            var mapperHelper = new TestMapperHelper();

            mapperMock
                .Setup(x => x.Map(It.IsAny<ImageRequest>()))
                .Returns(mapperHelper.GetImage());

            repositoryMock
                .Setup(x => x.AddNewImage(It.IsAny<DbImage>()))
                .Returns(secondDbImage.Id);

            repositoryMock
                .Setup(x => x.AddNewImage(firstDbImage))
                .Returns(firstDbImage.Id);

            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Returns(resizedImageContent);

            command = new AddNewImageCommand(repositoryMock.Object, validatorMock.Object, mapperMock.Object, algorithmMock.Object);
        }

        [Test]
        public void ShouldAddNewImageAndThumbImmage()
        {
            Assert.AreEqual(firstDbImage.Id, command.Execute(imageRequest));

            // Ok: validator -> mapper -> repository -> mapper -> algorithm -> repository

            validatorMock.Verify(v => v.Validate(It.IsAny<IValidationContext>()), Times.Once);
            repositoryMock.Verify(r => r.AddNewImage(It.IsAny<DbImage>()), Times.Exactly(2));
            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>()), Times.Exactly(2));
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
            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mapperMock.Verify(m => m.Map(It.IsAny<ImageRequest>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageRequestIsNull()
        {
            mapperMock
                 .Setup(x => x.Map(It.IsAny<ImageRequest>()))
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


        [Test]
        public void ShouldThrowExceptionWhenAlgorithmThrowsException()
        {
            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(imageRequest));
        }
    }
}