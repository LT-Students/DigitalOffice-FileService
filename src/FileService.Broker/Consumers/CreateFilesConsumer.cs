using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class CreateFilesConsumer : IConsumer<ICreateFilesPublish>
  {
    private readonly IFileRepository _repository;
    private readonly IDbFileMapper _mapper;

    private async Task<bool> CreateFilesAsync(ICreateFilesPublish request)
    {
      return await _repository.CreateAsync(request.Files.Select(x => _mapper.Map(x, request.CreatedBy)).ToList());
    }

    public CreateFilesConsumer(
      IFileRepository repository,
      IDbFileMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateFilesPublish> context)
    {
      object response = OperationResultWrapper.CreateResponse(CreateFilesAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
