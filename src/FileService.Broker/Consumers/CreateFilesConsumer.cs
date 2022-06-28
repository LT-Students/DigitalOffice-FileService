using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class CreateFilesConsumer : IConsumer<ICreateFilesPublish>
  {
    private readonly IFileRepository _repository;
    private readonly IDbFileMapper _mapper;

    public CreateFilesConsumer(
      IFileRepository repository,
      IDbFileMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateFilesPublish> context)
    {
      if (context.Message.Files is not null && context.Message.Files.Any())
      {
        await _repository.CreateAsync(context.Message.Files
          .Select(x => _mapper.Map(x, context.Message.CreatedBy)).ToList());
      }
    }
  }
}
