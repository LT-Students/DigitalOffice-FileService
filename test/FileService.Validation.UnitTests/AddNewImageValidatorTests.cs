using FluentValidation.TestHelper;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using NUnit.Framework;

namespace LT.DigitalOffice.FileService.Validation.UnitTests
{
    public class AddNewImageRequestValidatorTests
    {
        private IImageRequestValidator _validator;

        private ImageRequest _imageRequest;

        [SetUp]
        public void SetUp()
        {
            _imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_Photo"
            };

            _validator = new ImageRequestValidator();
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenFileIsValid()
        {
            _validator.TestValidate(_imageRequest).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsInWrongEncoding()
        {
            _imageRequest.Content = "T 1 ! * & ? Z :C ; _____";

            var fileValidationResult = _validator.TestValidate(_imageRequest);

            fileValidationResult.ShouldHaveValidationErrorFor(f => f.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsNull()
        {
            _imageRequest.Content = null;

            _validator.TestValidate(_imageRequest).ShouldHaveValidationErrorFor(request => request.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenExtensionIsIncorrect()
        {
            _imageRequest.Extension = ".mp3";
            _validator.TestValidate(_imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);

            _imageRequest.Extension = "abracadabra";
            _validator.TestValidate(_imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);

            _imageRequest.Extension = "";
            _validator.TestValidate(_imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);
        }
    }
}
