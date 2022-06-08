using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class CreateFilesConsumer : IConsumer<ICreateFilesPublish>
  {
    private readonly IFileRepository _repository;
    private readonly IDbFileMapper _mapper;
    private readonly ILogger<CreateFilesConsumer> _logger;

    private async Task CreateFilesAsync(ICreateFilesPublish request)
    {
      List<DbFile> notAdded = await _repository.CreateAsync(request.Files.Select(x => _mapper.Map(x, request.CreatedBy)).ToList());

      if (notAdded.Any())
      {
        _logger.LogWarning(
          "This files wasn't added to DB {notAdded}.",
          string.Join(',', notAdded));
      }
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
    }
  }
}
