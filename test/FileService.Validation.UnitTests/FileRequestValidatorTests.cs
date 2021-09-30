using FluentValidation.TestHelper;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using NUnit.Framework;

namespace LT.DigitalOffice.FileService.Validation.UnitTests
{
    public class FileRequestValidatorTests
    {
        private IFileRequestValidator _validator;

        private AddFileRequest _fileRequest;

        [SetUp]
        public void SetUp()
        {
            _fileRequest = new AddFileRequest
            {
                Content = "RGlnaXRhbCBPZmA5Y2U=",
                Extension = ".txt",
                Name = "DigitalOfficeTestFile"
            };

            _validator = new FileRequestValidator();
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenFileIsValid()
        {
            _validator.TestValidate(_fileRequest).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsInWrongEncoding()
        {
            _fileRequest.Content = "T 1 ! * & ? Z :C ; _____";

            var fileValidationResult = _validator.TestValidate(_fileRequest);

            fileValidationResult.ShouldHaveValidationErrorFor(f => f.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenNameIsTooLong()
        {
            _fileRequest.Name += _fileRequest.Name.PadLeft(244);

            var fileValidationResult = _validator.TestValidate(_fileRequest);

            fileValidationResult.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenNameIsEmpty()
        {
            _fileRequest.Name = "";

            _validator.TestValidate(_fileRequest).ShouldHaveValidationErrorFor(request => request.Name);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenContentIsNull()
        {
            _fileRequest.Content = null;

            _validator.TestValidate(_fileRequest).ShouldHaveValidationErrorFor(request => request.Content);
        }

        [Test]
        public void ShouldHaveValidationErrorWhenNameDoesNotMatchRegularExpression()
        {
            _fileRequest.Name = "???'";

            _validator.TestValidate(_fileRequest).ShouldHaveValidationErrorFor(request => request.Name);
        }
    }
}
