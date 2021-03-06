﻿using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Requests.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class AddImageConsumer : IConsumer<IAddImageRequest>
    {
        private readonly IAddImageCommand _command;
        private readonly IImageRequestMapper _mapper;

        private object GetImageId(IAddImageRequest request)
        {
            var imageRequest = _mapper.Map(request);

            var imageId = _command.Execute(imageRequest, request.UserId);

            return new
            {
                Id = imageId
            };
        }

        public AddImageConsumer(
            IAddImageCommand command,
            IImageRequestMapper mapper)
        {
            _command = command;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IAddImageRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImageId, context.Message);

            await context.RespondAsync<IOperationResult<IAddImageResponse>>(response);
        }
    }
}
