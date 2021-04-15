using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class AddImageConsumer : IConsumer<IAddImageRequest>
    {
        private readonly IAddNewImageCommand _command;
        private readonly IImageRequestMapper _mapper;

        private object GetImage(IAddImageRequest request)
        {
            var imageRequest = _mapper.Map(request);

            var imageId = _command.Execute(imageRequest, request.UserId);

            return new
            {
                Id = imageId
            };
        }

        public AddImageConsumer(
            IAddNewImageCommand command,
            IImageRequestMapper mapper)
        {
            _command = command;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IAddImageRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImage, context.Message);

            await context.RespondAsync<IOperationResult<IAddImageResponse>>(response);
        }
    }
}
