﻿using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.FileService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.FileService.Mappers.RequestMappers
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
