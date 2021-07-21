using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.FileService.Business.Commands.Image
{
    public class GetImageCommand : IGetImageCommand
    {
        private readonly IImageRepository _repository;
        private readonly IImageInfoMapper _mapper;

        public GetImageCommand(
            IImageRepository repository,
            IImageInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public OperationResultResponse<ImageInfo> Execute(Guid imageId)
        {
            return new OperationResultResponse<ImageInfo>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _mapper.Map(_repository.Get(imageId)),
                Errors = new()
            };
        }
    }
}
