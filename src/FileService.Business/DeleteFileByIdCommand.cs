using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    public class DeleteFileByIdCommand : IDeleteFileByIdCommand
    {
        private readonly IFileRepository repository;

        public DeleteFileByIdCommand([FromServices] IFileRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(Guid fileId)
        {
            repository.DisableFileById(fileId);
        }
    }
}
