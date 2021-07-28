using LT.DigitalOffice.FileService.Business.Commands.Image;
using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.FileService.Business.UnitTests.Commands.Image
{
    public class FindImagesCommandTests
    {
        private IFindImagesCommand _command;
        private AutoMocker _mocker;
        private DbImage _dbImage1;
        private DbImage _dbImage2;
        private ImageInfo _imageInfo1;
        private ImageInfo _imageInfo2;
        private List<Guid> _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mocker = new();

            _dbImage1 = new()
            {
                Id = Guid.NewGuid(),
                Name = "MyImage1",
                ParentId = Guid.NewGuid(),
                Content = "Content",
                Extension = ".jpg",
                ImageType = (int)ImageType.Thumb,
                UserId = Guid.NewGuid(),
                AddedOn = DateTime.UtcNow,
                IsActive = true
            };

            _imageInfo1 = new()
            {
                Id = _dbImage1.Id,
                ParentId = _dbImage1.ParentId,
                Content = _dbImage1.Content,
                Extension = _dbImage1.Extension,
                Name = _dbImage1.Name,
                Type = ImageType.Thumb
            };

            _dbImage2 = new()
            {
                Id = Guid.NewGuid(),
                Name = "MyImage2",
                ParentId = Guid.NewGuid(),
                Content = "Content",
                Extension = ".jpg",
                ImageType = (int)ImageType.Thumb,
                UserId = Guid.NewGuid(),
                AddedOn = DateTime.UtcNow,
                IsActive = true
            };

            _imageInfo2 = new()
            {
                Id = _dbImage1.Id,
                ParentId = _dbImage1.ParentId,
                Content = _dbImage1.Content,
                Extension = _dbImage1.Extension,
                Name = _dbImage1.Name,
                Type = ImageType.Thumb
            };

            _mocker
                .Setup<IImageInfoMapper, ImageInfo>(m => m.Map(_dbImage1))
                .Returns(_imageInfo1);

            _mocker
                .Setup<IImageInfoMapper, ImageInfo>(m => m.Map(_dbImage2))
                .Returns(_imageInfo2);
        }

        [SetUp]
        public void SetUp()
        {
            _request = new() { _dbImage1.Id, _dbImage2.Id };

            _command = _mocker.CreateInstance<FindImagesCommand>();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<IImageRepository, List<DbImage>>(r => r.Get(It.IsAny<List<Guid>>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(new List<Guid> { _dbImage1.Id, _dbImage2.Id }));
        }

        [Test]
        public void ShouldGetImagesSuccessfuly()
        {
            _mocker
                .Setup<IImageRepository, List<DbImage>>(r => r.Get(_request))
                .Returns(new List<DbImage> { _dbImage1, _dbImage2 });

            OperationResultResponse<List<ImageInfo>> result = new()
            {
                Body = new List<ImageInfo> { _imageInfo1, _imageInfo2 },
                Status = OperationResultStatusType.FullSuccess,
                Errors = new()
            };

            SerializerAssert.AreEqual(result, _command.Execute(_request));
        }

        [Test]
        public void ShouldGetPartOfImages()
        {
            _request.Add(Guid.NewGuid());

            _mocker
                .Setup<IImageRepository, List<DbImage>>(r => r.Get(_request))
                .Returns(new List<DbImage> { _dbImage1, _dbImage2 });

            OperationResultResponse<List<ImageInfo>> result = _command.Execute(_request);

            SerializerAssert.AreEqual(new List<ImageInfo> { _imageInfo1, _imageInfo2 }, result.Body);
            Assert.AreEqual(OperationResultStatusType.PartialSuccess, result.Status);
            Assert.IsNotEmpty(result.Errors);
        }
    }
}
