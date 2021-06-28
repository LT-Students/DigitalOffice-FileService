using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class GetImageConsumer : IConsumer<IGetImageRequest>
    {
        private readonly IImageRepository _repository;

        private object GetImage(IGetImageRequest request)
        {
            DbImage image = _repository.Get(request.ImageId);

            return IGetImageResponse.CreateObj(
                                        image.Id,
                                        image.ParentId,
                                        image.Content,
                                        image.Extension,
                                        image.Name);
        }

        public GetImageConsumer(
            IImageRepository repository)
        {
            _repository = repository;
        }

        public Task Consume(ConsumeContext<IGetImageRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetImage, context.Message);

            return context.RespondAsync<IOperationResult<IGetImageResponse>>(response);
        }

    }
}
