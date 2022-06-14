﻿using System.Linq;
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
    private readonly ILogger<CreateFilesConsumer> _logger;

    public CreateFilesConsumer(
      IFileRepository repository,
      IDbFileMapper mapper,
      ILogger<CreateFilesConsumer> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task Consume(ConsumeContext<ICreateFilesPublish> context)
    {
      await _repository.CreateAsync(context.Message.Files.Select(x => _mapper.Map(x, context.Message.CreatedBy)).ToList());
    }
  }
}
