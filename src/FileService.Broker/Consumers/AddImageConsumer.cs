using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Requests.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto.Enums;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.FileService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class AddImageConsumer : IConsumer<IAddImageRequest>
    {
        private readonly IImageRequestMapper _requestImageMapper;
        private readonly IImageRepository _repository;
        private readonly IImageRequestValidator _validator;
        private readonly IDbImageMapper _dbImageMapper;

        private Guid GetImageId(IAddImageRequest request)
        {
            AddImageRequest imageRequest = _requestImageMapper.Map(request);
            _validator.ValidateAndThrowCustom(imageRequest);

            DbImage parentDbImage = _dbImageMapper.Map(imageRequest, ImageType.Full, out bool isBigImage, request.UserId);
            DbImage childDbImage = null;

            if (isBigImage)
            {
                childDbImage = _dbImageMapper.Map(imageRequest, ImageType.Thumb, out _, request.UserId, parentDbImage.Id);
                _repository.Add(childDbImage);
            }

            _repository.Add(parentDbImage);

            return childDbImage == null ? parentDbImage.Id : childDbImage.Id;
        }

        public AddImageConsumer(
            IImageRepository repository,
            IImageRequestValidator validator,
            IDbImageMapper dbImageMapper,
            IImageRequestMapper requestImageMapper)
        {

            _requestImageMapper = requestImageMapper;
            _repository = repository;
            _validator = validator;
            _dbImageMapper = dbImageMapper;
        }

        public async Task Consume(ConsumeContext<IAddImageRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImageId, context.Message);

            await context.RespondAsync<IOperationResult<Guid>>(response);
        }
    }
}
