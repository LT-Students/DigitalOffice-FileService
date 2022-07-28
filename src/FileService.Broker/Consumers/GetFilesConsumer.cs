using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
  public class GetFilesConsumer : IConsumer<IGetFilesRequest>
  {    
    private readonly IFileRepository _repository;

    private async Task<object> GetFilesAsync(IGetFilesRequest request)
    {
      List<DbFile> files = await _repository.GetAsync(request.FilesIds);

      return IGetFilesResponse.CreateObj(files.Select(
       file =>
         new FileCharacteristicsData(
           file.Id,
           file.Extension,
           (int)file.Size,
           file.CreatedAtUtc
         )).ToList());
    }

    public GetFilesConsumer(IFileRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetFilesRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetFilesAsync, context.Message);

      await context.RespondAsync<IOperationResult<IGetFilesResponse>>(response);
    }
  }
}
