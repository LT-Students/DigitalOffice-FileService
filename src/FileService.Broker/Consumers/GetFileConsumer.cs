using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class GetFileConsumer : IConsumer<IGetFilesRequest>
    {
        private readonly IFileRepository _repository;
        private readonly IFileDataMapper _mapper;

        public GetFileConsumer(
            IFileRepository repository,
            IFileDataMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IGetFilesRequest> context)
        {
            object response = OperationResultWrapper.CreateResponse(GetFile, context.Message);

            await context.RespondAsync<IOperationResult<IGetFilesResponse>>(response);
        }

        private async Task<object> GetFile(IGetFilesRequest request)
        {
            List<FileData> files = (await _repository.GetAsync(request.FilesIds)).Select(x => _mapper.Map(x)).ToList();

            return IGetFilesResponse.CreateObj(files);
        }
    }
}