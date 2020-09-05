using LT.DigitalOffice.FileService.Commands.Interfaces;
using LT.DigitalOffice.FileService.Database.Entities;
using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models;
using LT.DigitalOffice.FileService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Commands
{
    public class GetFileByIdCommand : IGetFileByIdCommand
    {
        private readonly IFileRepository repository;
        private readonly IMapper<DbFile, File> mapper;

        public GetFileByIdCommand(
            [FromServices] IFileRepository repository,
            [FromServices] IMapper<DbFile, File> mapper)
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