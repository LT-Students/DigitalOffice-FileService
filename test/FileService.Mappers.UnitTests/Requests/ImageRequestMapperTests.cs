using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Mappers.Requests;
using LT.DigitalOffice.FileService.Mappers.Requests.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.FileService.Mappers.UnitTests.Requests
{
    public class ImageRequestMapperTests
    {
        private IImageRequestMapper _mapper;

        private Mock<IAddImageRequest> _addImageRequestMock;
        private ImageRequest _imageRequest;

        private Guid _userId;
        private const string _name = "Name";
        private const string _extension = "ext";
        private const string _content = "content";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userId = Guid.NewGuid();

            _mapper = new ImageRequestMapper();

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

            _imageRequest = new ImageRequest
            {
                Name = _name,
                Extension = _extension,
                Content = _content
            };
        }

        [Test]
        public void ShouldReturnCorrectImageRequest()
        {
            SerializerAssert.AreEqual(_imageRequest, _mapper.Map(_addImageRequestMock.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenAddImageRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }
    }
}
