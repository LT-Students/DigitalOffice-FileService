using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class RemoveFilesConsumer : IConsumer<IRemoveFilesRequest>
  {
    private readonly IFileRepository _repository;

    private async Task<bool> RemoveFilesAsync(IRemoveFilesRequest request)
    {
      return await _repository.RemoveAsync(request.FilesIds);
    }

    public RemoveFilesConsumer(
      IFileRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IRemoveFilesRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(RemoveFilesAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
