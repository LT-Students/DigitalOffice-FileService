using LT.DigitalOffice.FileService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.FileService.Business.Commands.Image
{
    public class FindImagesCommand : IFindImagesCommand
    {
        private readonly IImageRepository _repository;
        private readonly IImageInfoMapper _mapper;

        public FindImagesCommand(
            IImageRepository repository,
            IImageInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public OperationResultResponse<List<ImageInfo>> Execute(List<Guid> imageIds)
        {
            OperationResultResponse<List<ImageInfo>> result = new();

            result.Body = _repository.Get(imageIds).Select(_mapper.Map).ToList();

            if (result.Body.Count == imageIds.Count)
            {
                result.Status = OperationResultStatusType.FullSuccess;
                return result;
            }

            result.Status = OperationResultStatusType.PartialSuccess;
            result.Errors.Add($"No images with ids: '{string.Join("', '", imageIds.Where(id => !result.Body.Any(image => image.Id == id)))}'.");
            return result;
        }
    }
}
