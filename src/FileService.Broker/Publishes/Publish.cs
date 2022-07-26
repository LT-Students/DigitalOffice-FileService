using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using MassTransit;
using LT.DigitalOffice.FileService.Broker.Publishes.Interfaces;

namespace LT.DigitalOffice.FileService.Broker.Publishes
{
  public class Publish : IPublish
  {
    private readonly IBus _bus;

    public Publish(IBus bus)
    {
      _bus = bus;
    }

    public Task CreateFilesAsync(Guid entityId, FileAccessType access, List<Guid> filesIds)
    {
      return _bus.Publish<ICreateFilesPublish>(ICreateFilesPublish.CreateObj(
        filesIds: filesIds,
        access: access,
        projectId: entityId));
    }
  }
}
