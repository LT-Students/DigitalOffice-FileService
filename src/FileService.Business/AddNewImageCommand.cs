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
        private readonly IImageRepository repository;
        private readonly IValidator<ImageRequest> validator;
        private readonly IDbImageMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddNewImageCommand(
            IImageRepository repository,
            IValidator<ImageRequest> validator,
            IDbImageMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Execute(ImageRequest request, Guid? userId = null)
        {
            validator.ValidateAndThrowCustom(request);

            Guid requiredUserId = userId ?? _httpContextAccessor.HttpContext.GetUserId();

            var parentDbImage = mapper.Map(request, ImageType.Full, requiredUserId);
            repository.AddNewImage(parentDbImage);

            var childDbImage = mapper.Map(request, ImageType.Thumb, requiredUserId, parentDbImage.Id);
            repository.AddNewImage(childDbImage);

            return parentDbImage.Id;
        }
    }
}
