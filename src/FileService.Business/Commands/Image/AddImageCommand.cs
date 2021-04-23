using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.Image
{
    public class AddImageCommand : IAddImageCommand
    {
        private readonly IImageRepository _repository;
        private readonly IImageRequestValidator _validator;
        private readonly IDbImageMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddImageCommand(
            IImageRepository repository,
            IImageRequestValidator validator,
            IDbImageMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Execute(ImageRequest request, Guid? userId = null)
        {
            _validator.ValidateAndThrowCustom(request);

            Guid requiredUserId = userId ?? _httpContextAccessor.HttpContext.GetUserId();

            var parentDbImage = _mapper.Map(request, ImageType.Full, out bool isBigImage, requiredUserId);

            if (isBigImage)
            {
                var childDbImage = _mapper.Map(request, ImageType.Thumb, out _, requiredUserId, parentDbImage.Id);
                _repository.AddImage(childDbImage);
            }

            _repository.AddImage(parentDbImage);

            return parentDbImage.Id;
        }
    }
}
