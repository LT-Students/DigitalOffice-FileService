using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class GetImagesConsumer : IConsumer<IGetImagesRequest>
    {
        private readonly IImageRepository _repository;

        private object GetImages(IGetImagesRequest requets)
        {
            return IGetImagesResponse.CreateObj(
                _repository.Get(requets.ImageIds)
                    .Select(image => new ImageData(image.Id, image.ParentId, image.Content, image.Extension, image.Name)).ToList());
        }

        public GetImagesConsumer(IImageRepository imageRepository)
        {
            _repository = imageRepository;
        }

        public async Task Consume(ConsumeContext<IGetImagesRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImages, context.Message);

            await context.RespondAsync<IOperationResult<IGetImagesResponse>>(response);
        }
    }
}
