using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests
{
    public class DbImageMapperTests
    {
        private IDbImageMapper _mapper;
        private Mock<IImageResizeAlgorithm> _algorithmMock;

        private ImageRequest _imageRequest;
        private DbImage _dbImage;
        private string _resizedImageContent;
        private string _content = Properties.Resources.SmallImageContent;
        private string _bigContent = Properties.Resources.BigImageContent;
        private Guid _parentId;
        private Guid _userId;
        private bool isBigImage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid();
            _parentId = Guid.NewGuid();

            _resizedImageContent = "abracadabra";

            _algorithmMock = new Mock<IImageResizeAlgorithm>();
            _algorithmMock
                .Setup(x => x.Resize(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_resizedImageContent);

            _mapper = new DbImageMapper(_algorithmMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _imageRequest = new ImageRequest
            {
                Content = _content,
                Extension = ".png",
                Name = "Spartak_Photo",
            };

            _dbImage = new DbImage()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Content = _content,
                Extension = _imageRequest.Extension.ToLower(),
                Name = _imageRequest.Name,
                AddedOn = DateTime.Now,
                UserId = _userId,
                ImageType = (int)ImageType.Full,
                IsActive = true
            };
        }

        [Test]
        public void ShouldReturnCorrectSmallFullDbImage()
        {
            var result = _mapper.Map(
                _imageRequest,
                ImageType.Full,
                out isBigImage,
                _userId);

            _dbImage.Id = result.Id;
            _dbImage.AddedOn = result.AddedOn;

            SerializerAssert.AreEqual(_dbImage, result);
            Assert.IsFalse(isBigImage);
        }

        [Test]
        public void ShouldReturnCorrectBigFullDbImage()
        {
            _imageRequest.Content = _bigContent;

            var result = _mapper.Map(
                _imageRequest,
                ImageType.Full,
                out isBigImage,
                _userId);

            _dbImage.Id = result.Id;
            _dbImage.AddedOn = result.AddedOn;
            _dbImage.Content = _bigContent;

            SerializerAssert.AreEqual(_dbImage, result);
            Assert.IsTrue(isBigImage);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenTypeFullAndParentInNotNull()
        {
            Assert.Throws<ArgumentException>(() => _mapper.Map(
                _imageRequest,
                ImageType.Full,
                out isBigImage,
                _userId,
                _parentId));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenImageRequestNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(
                null,
                ImageType.Full,
                out isBigImage,
                _userId,
                _parentId));
        }

        [Test]
        public void ShouldReturnCorrectThumbDbImage()
        {
            var result = _mapper.Map(
                _imageRequest,
                ImageType.Thumb,
                out isBigImage,
                _userId,
                _parentId);

            _dbImage.Id = result.Id;
            _dbImage.AddedOn = result.AddedOn;
            _dbImage.Content = _resizedImageContent;
            _dbImage.ParentId = _parentId;
            _dbImage.ImageType = (int)ImageType.Thumb;

            SerializerAssert.AreEqual(_dbImage, result);
        }
    }
}