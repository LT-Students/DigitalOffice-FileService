using FluentValidation;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    public class AddNewImageCommand : IAddNewImageCommand
    {
        private readonly IImageRepository _repository;
        private readonly IValidator<ImageRequest> _validator;
        private readonly IDbImageMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddNewImageCommand(
            IImageRepository repository,
            IValidator<ImageRequest> validator,
            IDbImageMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this._repository = repository;
            this._validator = validator;
            this._mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Execute(ImageRequest request, Guid? userId = null)
        {
            _validator.ValidateAndThrowCustom(request);

            Guid requiredUserId = userId ?? _httpContextAccessor.HttpContext.GetUserId();

            var parentDbImage = _mapper.Map(request, ImageType.Full, requiredUserId);
            _repository.AddNewImage(parentDbImage);

            var childDbImage = _mapper.Map(request, ImageType.Thumb, requiredUserId, parentDbImage.Id);
            _repository.AddNewImage(childDbImage);

            return parentDbImage.Id;
        }
    }
}
