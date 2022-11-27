using System.Collections.Generic;
using System.IO;
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
      List<string> removedFiles = await _repository.RemoveAsync(context.Message.FileSource, context.Message.FilesIds);

      foreach (string path in removedFiles)
      {
        if (File.Exists($"{path}"))
        {
          File.Delete($"{path}");
        }
      }

      if (removedFiles.Count != context.Message.FilesIds.Count)
      {
        _logger.LogWarning(
          "Files ids: {FilesIds} were not removed.",
          string.Join(",", context.Message.FilesIds.Where(f => !context.Message.FilesIds.Contains(f))));
      }
    }
  }
}
