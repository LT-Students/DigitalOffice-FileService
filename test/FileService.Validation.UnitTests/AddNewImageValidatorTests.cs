using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using NUnit.Framework;

namespace LT.DigitalOffice.FileService.Validation.UnitTests
{
    public class AddNewImageRequestValidatorTests
    {
        private IValidator<ImageRequest> validator;

        private ImageRequest imageRequest;

        [SetUp]
        public void SetUp()
        {
            imageRequest = new ImageRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".png",
                Name = "Spartak_Photo"
            };

            validator = new ImageRequestValidator();
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenFileIsValid()
        {
            validator.TestValidate(imageRequest).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsInWrongEncoding()
        {
            imageRequest.Content = "T 1 ! * & ? Z :C ; _____";

            var fileValidationResult = validator.TestValidate(imageRequest);

            fileValidationResult.ShouldHaveValidationErrorFor(f => f.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsNull()
        {
            imageRequest.Content = null;

            validator.TestValidate(imageRequest).ShouldHaveValidationErrorFor(request => request.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenExtensionIsIncorrect()
        {
            imageRequest.Extension = ".mp3";
            validator.TestValidate(imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);

            imageRequest.Extension = "abracadabra";
            validator.TestValidate(imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);

            imageRequest.Extension = "";
            validator.TestValidate(imageRequest).ShouldHaveValidationErrorFor(request => request.Extension);
        }
    }
}
