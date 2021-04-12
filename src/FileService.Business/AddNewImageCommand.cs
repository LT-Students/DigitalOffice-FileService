using FluentValidation;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    public class AddNewImageCommand : IAddNewImageCommand
    {
        private readonly IImageRepository _repository;
        private readonly IValidator<ImageRequest> _validator;
        private readonly IImageRequestMapper _mapper;
        public AddNewImageCommand(
               IImageRepository repository,
               IValidator<ImageRequest> validator,
               IImageRequestMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public Guid Execute(ImageRequest request)
        {
            _validator.ValidateAndThrowCustom(request);

            var parentDbImage = _mapper.Map(request, ImageType.Full, out bool isBigImage);

            if (isBigImage)
            {
                var childDbImage = _mapper.Map(request, ImageType.Thumb, out isBigImage, parentDbImage.Id);
                _repository.AddNewImage(childDbImage);
            }
            else
            {
                isBigImage = false;
                _repository.AddNewImage(parentDbImage);
            }
            return parentDbImage.Id;
        }
    }
}
