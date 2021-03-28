using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public FakeHttpContextAccessor(Guid userId)
        {
            HttpContext = new FakeHttpContext(userId);
        }
    }

    public class FakeHttpContext : HttpContext
    {
        public FakeHttpContext(Guid userId)
        {
            Items = new Dictionary<object, object>
            {
                {
                    ConstStrings.UserId, userId.ToString()
                }
            };
        }

        public override IDictionary<object, object> Items { get; set; }

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => throw new NotImplementedException();

        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override AuthenticationManager Authentication => throw new NotImplementedException();

        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }

    public class ImageMapperTests
    {
        private IImageRequestMapper requestToDbMapper;
        private Mock<IImageResizeAlgorithm> algorithmMock;
        private FakeHttpContextAccessor fakeHttpContextAccessor;

        private ImageRequest imageRequest;
        private byte[] resizedImageContent;
        private Guid parentId;
        private Guid userId;

        [SetUp]
        public void SetUp()
        {
            userId = Guid.NewGuid();
            algorithmMock = new Mock<IImageResizeAlgorithm>();
            fakeHttpContextAccessor = new FakeHttpContextAccessor(userId);
            requestToDbMapper = new ImageRequestMapper(algorithmMock.Object, fakeHttpContextAccessor);

            imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_Photo",
            };

            resizedImageContent = new byte[] { 0, 1, 1, 0 };

            parentId = Guid.NewGuid();

            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Returns(resizedImageContent);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestMappingObjectIsNull()
        {
            imageRequest = null;

            Assert.Throws<ArgumentNullException>(() => requestToDbMapper.Map(imageRequest, ImageType.Full));
            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequestAndImageTypeIsFull()
        {
            var newImage = requestToDbMapper.Map(imageRequest, ImageType.Full);

            var expectedImage = new DbImage
            {
                Id = newImage.Id,
                Content = Convert.FromBase64String(imageRequest.Content),
                Extension = imageRequest.Extension,
                Name = imageRequest.Name,
                UserId = userId,
                ImageType = (int)ImageType.Full,
                AddedOn = newImage.AddedOn,
                IsActive = true
            };

            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Never);


            SerializerAssert.AreEqual(expectedImage, newImage);
        }

        [Test]
        public void ShouldReturnDbFileWhenMappingFileRequestAndImageTypeIsThumb()
        {
            var newImage = requestToDbMapper.Map(imageRequest, ImageType.Thumb, parentId);

            var expectedImage = new DbImage
            {
                Id = newImage.Id,
                Content = resizedImageContent,
                Extension = imageRequest.Extension,
                Name = imageRequest.Name,
                UserId = userId,
                ImageType = (int)ImageType.Thumb,
                AddedOn = newImage.AddedOn,
                IsActive = true,
                ParentId = parentId
            };

            algorithmMock.Verify(a => a.Resize(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            SerializerAssert.AreEqual(expectedImage, newImage);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageTypeIsFullButParentIdIsNotNull()
        {
            Assert.Throws<ArgumentException>(() => requestToDbMapper.Map(imageRequest, ImageType.Full, parentId));
        }

        [Test]
        public void ShouldThrowExceptionWhenAlgorithmThrowsExceptionAndImageTypeIsThumbs()
        {
            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => requestToDbMapper.Map(imageRequest, ImageType.Thumb));
        }

        [Test]
        public void ShouldNotThrowExceptionWhenAlgorithmThrowsExceptionAndImageTypeIsFull()
        {
            algorithmMock
                .Setup(x => x.Resize(imageRequest.Content, imageRequest.Extension))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => requestToDbMapper.Map(imageRequest, ImageType.Thumb));
        }
    }
}