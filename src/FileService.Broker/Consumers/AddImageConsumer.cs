using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class AddImageConsumer : IConsumer<IAddImageRequest>
    {
        private readonly IAddNewImageCommand _command;
        private readonly IImageRequestMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        private object GetImage(IAddImageRequest request)
        {
            var imageRequest = _mapper.Map(request);

            IDictionary<object, object> httpContextItems = new Dictionary<object, object>();

            httpContextItems.Add(ConstStrings.UserId, request.UserId);
            _contextAccessor.HttpContext.Items = httpContextItems;

            var imageId = _command.Execute(imageRequest);

            return new
            {
                Id = imageId
            };
        }

        public AddImageConsumer(
            IAddNewImageCommand command,
            IImageRequestMapper mapper,
            IHttpContextAccessor accessor)
        {
            _command = command;
            _mapper = mapper;
            _contextAccessor = accessor;
        }

        public async Task Consume(ConsumeContext<IAddImageRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImage, context.Message);

            await context.RespondAsync<IOperationResult<IAddImageRequest>>(response);
        }
    }
}
