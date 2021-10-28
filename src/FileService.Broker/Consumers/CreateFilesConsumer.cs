using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class CreateFilesConsumer : IConsumer<ICreateFilesRequest>
    {
        private readonly IFileRepository _repository;
        private readonly IDbFileMapper _mapper;

        private async Task<bool> AddFiles(ICreateFilesRequest request)
        {
            return await _repository.CreateAsync(request.Files.Select(x => _mapper.Map(x, request.CreatedBy)).ToList());
        }

        public CreateFilesConsumer(
            IFileRepository repository,
            IDbFileMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ICreateFilesRequest> context)
        {
            object response = OperationResultWrapper.CreateResponse(AddFiles, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
