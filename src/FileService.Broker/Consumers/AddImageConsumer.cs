using LT.DigitalOffice.Broker.Requests;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class AddImageConsumer : IConsumer<IAddImageRequest>
    {
        public Task Consume(ConsumeContext<IAddImageRequest> context)
        {
            throw new NotImplementedException();
        }
    }
}
