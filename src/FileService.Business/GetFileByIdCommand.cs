using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    /// <inheritdoc cref="IGetFileByIdCommand"/>
    public class GetFileByIdCommand : IGetFileByIdCommand
    {
        private readonly IFileRepository repository;
        private readonly IFileMapper mapper;

        public GetFileByIdCommand(
            [FromServices] IFileRepository repository,
            [FromServices] IFileMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public File Execute(Guid fileId)
        {
            return mapper.Map(repository.GetFileById(fileId));
        }
    }
}