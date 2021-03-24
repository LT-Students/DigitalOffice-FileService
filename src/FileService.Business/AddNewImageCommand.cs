using FluentValidation;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;

namespace LT.DigitalOffice.FileService.Business
{
    public class AddNewImageCommand : IAddNewImageCommand
    {
        private readonly IImageRepository repository;
        private readonly IValidator<ImageRequest> validator;
        private readonly IMapper<ImageRequest, DbImage> mapper;
        private readonly IImageResizeAlgorithm resizeAlgotithm;

        public AddNewImageCommand(
            [FromServices] IImageRepository repository,
            [FromServices] IValidator<ImageRequest> validator,
            [FromServices] IMapper<ImageRequest, DbImage> mapper,
            [FromServices] IImageResizeAlgorithm resizeAlgotithm)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
            this.resizeAlgotithm = resizeAlgotithm;
        }

        public Guid Execute(ImageRequest request)
        {
            validator.ValidateAndThrowCustom(request);

            var parentDbImage = mapper.Map(request);
            repository.AddNewImage(parentDbImage);

            var childDbImage = mapper.Map(request);
            childDbImage.Content = resizeAlgotithm.Resize(request.Content, childDbImage.Extension);
            childDbImage.ParentId = parentDbImage.Id;
            childDbImage.ImageType = (int)ImageType.Thumbs;
            repository.AddNewImage(childDbImage);

            return parentDbImage.Id;
        }
    }
}
