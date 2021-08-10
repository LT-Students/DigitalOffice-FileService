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

namespace LT.DigitalOffice.FileService.Business.UnitTests.Commands.Image
{
    public class GetImageCommandTests
    {
        private IGetImageCommand _command;
        private AutoMocker _mocker;
        private DbImage _dbImage;
        private ImageInfo _imageInfo;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dbImage = new()
            {
                Id = Guid.NewGuid(),
                Name = "MyImage",
                ParentId = Guid.NewGuid(),
                Content = "Content",
                Extension = ".jpg",
                ImageType = (int)ImageType.Thumb,
                UserId = Guid.NewGuid(),
                AddedOn = DateTime.UtcNow,
                IsActive = true
            };

            _imageInfo = new()
            {
                Id = _dbImage.Id,
                ParentId = _dbImage.ParentId,
                Content = _dbImage.Content,
                Extension = _dbImage.Extension,
                Name = _dbImage.Name,
                Type = ImageType.Thumb
            };
        }

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _command = _mocker.CreateInstance<GetImageCommand>();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<IImageRepository, DbImage>(r => r.Get(It.IsAny<Guid>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_dbImage.Id));
        }

        [Test]
        public void ShouldGetImageSuccessfuly()
        {
            _mocker
                .Setup<IImageRepository, DbImage>(r => r.Get(_dbImage.Id))
                .Returns(_dbImage);

            _mocker
                .Setup<IImageInfoMapper, ImageInfo>(m => m.Map(_dbImage))
                .Returns(_imageInfo);

            OperationResultResponse<ImageInfo> result = new()
            {
                Body = _imageInfo,
                Status = OperationResultStatusType.FullSuccess,
                Errors = new()
            };

            SerializerAssert.AreEqual(result, _command.Execute(_dbImage.Id));
        }
    }
}
