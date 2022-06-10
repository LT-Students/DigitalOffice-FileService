using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class RemoveFilesConsumer : IConsumer<IRemoveFilesPublish>
  {
    private readonly IFileRepository _repository;
    private readonly ILogger<RemoveFilesConsumer> _logger;

    public RemoveFilesConsumer(
      IFileRepository repository,
      ILogger<RemoveFilesConsumer> logger)
    {
      _repository = repository;
      _logger = logger;
    }

    public async Task Consume(ConsumeContext<IRemoveFilesPublish> context)
    {
      List<Guid> notRemoved = await _repository.RemoveAsync(context.Message.FilesIds);

      if (notRemoved.Any())
      {
        _logger.LogWarning(
          "This files wasn't removed from FileServiceDb {notRemoved}.",
          string.Join(',', notRemoved));
      }
    }
  }
}
