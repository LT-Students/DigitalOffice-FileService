using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class RemoveFilesConsumer : IConsumer<IRemoveFilesPublish>
  {
    private readonly IFileRepository _repository;

    private async Task<bool> RemoveFilesAsync(IRemoveFilesPublish request)
    {
      return await _repository.RemoveAsync(request.FilesIds);
    }

    public RemoveFilesConsumer(
      IFileRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IRemoveFilesPublish> context)
    {
      object response = OperationResultWrapper.CreateResponse(RemoveFilesAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
