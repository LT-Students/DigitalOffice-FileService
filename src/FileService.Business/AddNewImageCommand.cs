using FluentValidation;
using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    public class AddNewImageCommand : IAddNewImageCommand
    {
        private readonly IImageRepository repository;
        private readonly IValidator<ImageRequest> validator;
        private readonly IImageRequestMapper mapper;

        public AddNewImageCommand(
            [FromServices] IImageRepository repository,
            [FromServices] IValidator<ImageRequest> validator,
            [FromServices] IImageRequestMapper mapper)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
        }

        public Guid Execute(ImageRequest request)
        {
            validator.ValidateAndThrowCustom(request);

            var parentDbImage = mapper.Map(request, ImageType.Full);

            repository.AddNewImage(parentDbImage);

            var childDbImage = mapper.Map(request, ImageType.Thumb);

            repository.AddNewImage(childDbImage);

            return parentDbImage.Id;
        }
    }
}
