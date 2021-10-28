using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class CreateFilesConsumer : IConsumer<ICreateFilesRequest>
    {
        public async Task Consume(ConsumeContext<ICreateFilesRequest> context)
        {
            object response = OperationResultWrapper.CreateResponse(AddFiles, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }

        public Task<bool> AddFiles(ICreateFilesRequest request)
        {
            throw new NullReferenceException();
      //      return true;
        }
    }
}
