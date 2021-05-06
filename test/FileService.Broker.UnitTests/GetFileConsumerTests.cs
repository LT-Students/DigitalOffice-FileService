using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.FileService.Broker.Consumers;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
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
    internal class GetFileConsumerTests
    {
        private string _contentString;
        private string _extension;
        private string _name;
        private Guid _fileId;

        private InMemoryTestHarness _harness;
        private Mock<IFileRepository> _repository;
        private IRequestClient<IGetFileRequest> _requestClient;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IFileRepository>();

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new GetFileConsumer(_repository.Object));

            _fileId = Guid.NewGuid();
            _contentString = "RGlnaXRhbCBPZmA5Y2U=";
            _extension = ".jpg";
            _name = "File";
        }

        [Test]
        public async Task ShouldResponseGetFileResponse()
        {
            await _harness.Start();

            _repository
                .Setup(x => x.GetFile(It.IsAny<Guid>()))
                .Returns(new DbFile
                {
                    Content = _contentString,
                    Extension = _extension,
                    Name = _name
                });

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetFileRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFileResponse>>(new
                {
                    FileId = _fileId
                });

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        Content = _contentString,
                        Extension = _extension,
                        Name = _name
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
        public async Task ShouldResponseIOperationResultWithExceptionWhenRepositoryNotFoundFile()
        {
            await _harness.Start();

            _repository
                .Setup(x => x.GetFile(It.IsAny<Guid>()))
                .Throws(new Exception("File with this id was not found."));

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetFileRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFileResponse>>(new
                {
                    FileId = _fileId
                });

                var expected = new
                {
                    IsSuccess = false,
                    Errors = new List<string> { "File with this id was not found." },
                    Body = null as object
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}