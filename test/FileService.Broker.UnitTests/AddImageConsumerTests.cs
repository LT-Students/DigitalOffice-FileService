using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.FileService.Broker.Consumers;
using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Requests.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.UnitTests
{
    class AddImageConsumerTests
    {
        private InMemoryTestHarness _harness;
        private Mock<IAddNewImageCommand> _commandMock;
        private Mock<IImageRequestMapper> _mapperMock;
        private IRequestClient<IAddImageRequest> _requestClient;
        private Mock<IAddImageRequest> _addImageRequestMock;
        private ImageRequest _imageRequest;

        private Guid _imageId;
        private Guid _userId;
        private const string _name = "Name";
        private const string _extension = "ext";
        private const string _content = "content";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _imageId = Guid.NewGuid();
            _userId = Guid.NewGuid();

            _imageRequest = new ImageRequest
            {
                Name = _name,
                Extension = _extension,
                Content = _content
            };

            _addImageRequestMock = new Mock<IAddImageRequest>();
            _addImageRequestMock
                .Setup(x => x.Content)
                .Returns(_content);
            _addImageRequestMock
                .Setup(x => x.Extension)
                .Returns(_extension);
            _addImageRequestMock
                .Setup(x => x.Name)
                .Returns(_name);
            _addImageRequestMock
                .Setup(x => x.UserId)
                .Returns(_userId);
        }

        [SetUp]
        public void SetUp()
        {
            _commandMock = new Mock<IAddNewImageCommand>();
            _commandMock
                .Setup(x => x.Execute(It.IsAny<ImageRequest>(), It.IsAny<Guid?>()))
                .Returns(_imageId);

            _mapperMock = new Mock<IImageRequestMapper>();
            _mapperMock
                .Setup(x => x.Map(It.IsAny<IAddImageRequest>()))
                .Returns(_imageRequest);

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new AddImageConsumer(
                    _commandMock.Object,
                    _mapperMock.Object));
        }

        [Test]
        public async Task ShouldSuccessResponse()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IAddImageRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IAddImageResponse>>(new
                {
                    Name = _name,
                    Content = _content,
                    Extension = _extension,
                    UserId = _userId
                });

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        Id = _imageId
                    }
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldResponseIOperationResultWithExceptionWhenCommandThrowException()
        {
            _commandMock
                .Setup(x => x.Execute(It.IsAny<ImageRequest>(), It.IsAny<Guid?>()))
                .Throws(new Exception());

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IAddImageRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IAddImageResponse>>(new
                {
                    Name = _name,
                    Content = _content,
                    Extension = _extension,
                    UserId = _userId
                });

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsNotEmpty(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldResponseIOperationResultWithExceptionWhenMapperThrowException()
        {
            _mapperMock
                .Setup(x => x.Map(It.IsAny<IAddImageRequest>()))
                .Throws(new ArgumentNullException());

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IAddImageRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IAddImageResponse>>(new
                {
                    Name = _name,
                    Content = _content,
                    Extension = _extension,
                    UserId = _userId
                });

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsNotEmpty(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
