using FluentValidation;
using LT.DigitalOffice.FileService.Business.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats;
using LT.DigitalOffice.Kernel.Exceptions;

namespace LT.DigitalOffice.FileService.Business
{
    public class AddNewImageCommand : IAddNewImageCommand
    {
        private readonly IImageRepository repository;
        private readonly IValidator<ImageRequest> validator;
        private readonly IImageMapper mapper;
        private readonly IImageResizeAlgorithm resizeAlgotithm;

        private static readonly Dictionary<string, IImageFormat> formats = new Dictionary<string, IImageFormat>
        {
            { ".jpg", JpegFormat.Instance },
            { ".jpeg", JpegFormat.Instance },
            { ".png", PngFormat.Instance },
            { ".bmp", BmpFormat.Instance },
            { ".gif", GifFormat.Instance },
            { ".tga", TgaFormat.Instance }
        };

        public AddNewImageCommand(
            [FromServices] IImageRepository repository,
            [FromServices] IValidator<ImageRequest> validator,
            [FromServices] IImageMapper mapper,
            [FromServices] IImageResizeAlgorithm resizeAlgotithm)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
            this.resizeAlgotithm = resizeAlgotithm;
        }

        public Guid Execute(ImageRequest request)
        {
            validator.ValidateAndThrowCustom(request);

            var parentDbImage = mapper.Map(request);
            repository.AddNewImage(parentDbImage);

            var childDbImage = mapper.Map(request);
            childDbImage.Content = resizeAlgotithm.Resize(request.Content, childDbImage.Extension);
            childDbImage.ParentId = parentDbImage.Id;
            repository.AddNewImage(childDbImage);

            return parentDbImage.Id;
        }

        public interface IImageResizeAlgorithm
        {
            public byte[] Resize(string base64String, string outputExtension);
        }

        public class ImageToSquareAlgorithm : IImageResizeAlgorithm
        {
            public byte[] Resize(string base64String, string outputExtension)
            {
                {
                    try
                    {
                        using (Image image = Image.Load(base64String))
                        {
                            // Trim the image in the center.
                            var minSize = Math.Min(image.Width, image.Height);
                            var offsetX = (image.Width - minSize) / 2;
                            var offsetY = (image.Height - minSize) / 2;
                            image.Mutate(x => x.Crop(new Rectangle(offsetX, offsetY, minSize, minSize)));

                            image.Mutate(x => x.Resize(150, 150));

                            return Convert.FromBase64String(image.ToBase64String(formats[outputExtension]));
                        }
                    }
                    catch
                    {
                        throw new BadRequestException("The server couldn't process the image.");
                    }
                }
            }
        }
    }
}
