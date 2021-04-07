﻿using LT.DigitalOffice.Broker.Requests;
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
        private string contentString;
        private string extension;
        private string name;
        private Guid fileId;

        private InMemoryTestHarness harness;
        private Mock<IFileRepository> repository;
        private IRequestClient<IGetFileRequest> requestClient;

        [SetUp]
        public void SetUp()
        {
            repository = new Mock<IFileRepository>();

            harness = new InMemoryTestHarness();
            var consumerTestHarness = harness.Consumer(() =>
                new GetFileConsumer(repository.Object));

            fileId = Guid.NewGuid();
            contentString = "RGlnaXRhbCBPZmA5Y2U=";
            extension = ".jpg";
            name = "File";
        }

        [Test]
        public async Task ShouldResponseGetFileResponse()
        {
            await harness.Start();

            repository
                .Setup(x => x.GetFileById(It.IsAny<Guid>()))
                .Returns(new DbFile
                {
                    Content = contentString,
                    Extension = extension,
                    Name = name
                });

            try
            {
                requestClient = await harness.ConnectRequestClient<IGetFileRequest>();

                var response = await requestClient.GetResponse<IOperationResult<IFileResponse>>(new
                {
                    FileId = fileId
                });

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        Content = contentString,
                        Extension = extension,
                        Name = name
                    }
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Test]
        public async Task ShouldResponseIOperationResultWithExceptionWhenRepositoryNotFoundFile()
        {
            await harness.Start();

            repository
                .Setup(x => x.GetFileById(It.IsAny<Guid>()))
                .Throws(new Exception("File with this id was not found."));

            try
            {
                requestClient = await harness.ConnectRequestClient<IGetFileRequest>();

                var response = await requestClient.GetResponse<IOperationResult<IFileResponse>>(new
                {
                    FileId = fileId
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
                await harness.Stop();
            }
        }
    }
}