using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.FileService.Broker.Consumers;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.UnitTests
{
    class AddImageConsumerTests
    {
        private string contentString;
        private string extension;
        private string name;
        private Guid fileId;

        private InMemoryTestHarness _harness;
        private Mock<IAddNewImageCommand> _command;
        private Mock<IImageRequestMapper> _mapperMock;
        private Mock<HttpContext> _contextMock;
        private IRequestClient<IAddImageRequest> _requestClient;

        [SetUp]
        public void SetUp()
        {
            _command = new Mock<IAddNewImageCommand>();

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new AddImageConsumer(
                    _command.Object,
                    _mapperMock.Object,
                    _contextMock.Object));

            fileId = Guid.NewGuid();
            contentString = "RGlnaXRhbCBPZmA5Y2U=";
            extension = ".jpg";
            name = "File";
        }

        [Test]
        public async Task ShouldResponseGetFileResponse()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IAddImageRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IAddImageResponse>>(new
                {
                    FileId = fileId
                });

            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldResponseIOperationResultWithExceptionWhenRepositoryNotFoundFile()
        {
            await _harness.Start();

            try
            {

            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
