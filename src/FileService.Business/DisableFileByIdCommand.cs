using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.FileService.Business
{
    public class DisableFileByIdCommand : IDisableFileByIdCommand
    {
        private readonly IFileRepository repository;

        public DisableFileByIdCommand([FromServices] IFileRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(Guid fileId)
        {
            repository.DisableFileById(fileId);
        }
    }
}
