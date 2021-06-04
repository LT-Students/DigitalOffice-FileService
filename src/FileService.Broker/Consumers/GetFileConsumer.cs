﻿using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Broker.Consumers
{
    public class GetFileConsumer : IConsumer<IGetFileRequest>
    {
        private readonly IFileRepository _repository;

        public GetFileConsumer(IFileRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetFileRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetFile, context.Message);

            await context.RespondAsync<IOperationResult<IFileResponse>>(response);
        }

        private object GetFile(IGetFileRequest request)
        {
            var dbFile = _repository.GetFile(request.FileId);

            return new
            {
                Content = dbFile.Content,
                Extension = dbFile.Extension,
                Name = dbFile.Name
            };
        }
    }
}