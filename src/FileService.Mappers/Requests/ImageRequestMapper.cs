using LT.DigitalOffice.FileService.Mappers.Requests.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Models.Broker.Requests.File;
using System;

namespace LT.DigitalOffice.FileService.Mappers.Requests
{
    public class ImageRequestMapper : IImageRequestMapper
    {
        public ImageRequest Map(IAddImageRequest imageRequest)
        {
            if (imageRequest == null)
            {
                throw new ArgumentNullException(nameof(imageRequest));
            }

            return new ImageRequest
            {
                Name = imageRequest.Name,
                Content = imageRequest.Content,
                Extension = imageRequest.Extension
            };
        }
    }
}
