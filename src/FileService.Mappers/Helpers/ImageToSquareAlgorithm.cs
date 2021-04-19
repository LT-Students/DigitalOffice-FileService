using LT.DigitalOffice.FileService.Mappers.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LT.DigitalOffice.FileService")]
namespace LT.DigitalOffice.FileService.Mappers.Helpers
{
    internal class ImageToSquareAlgorithm : IImageResizeAlgorithm
    {
        private static readonly Dictionary<string, IImageFormat> imageFormats = new()
        {
            { ".jpg", JpegFormat.Instance },
            { ".jpeg", JpegFormat.Instance },
            { ".png", PngFormat.Instance },
            { ".bmp", BmpFormat.Instance },
            { ".gif", GifFormat.Instance },
            { ".tga", TgaFormat.Instance }
        };

        public string Resize(string base64String, string outputExtension)
        {
            try
            {
                byte[] byteString = Convert.FromBase64String(base64String);
                Image image = Image.Load(byteString);

                var minSize = Math.Min(image.Width, image.Height);
                var offsetX = (image.Width - minSize) / 2;
                var offsetY = (image.Height - minSize) / 2;

                image.Mutate(x => x.Crop(new Rectangle(offsetX, offsetY, minSize, minSize)));

                image.Mutate(x => x.Resize(150, 150));

                return image.ToBase64String(imageFormats[outputExtension]);
            }
            catch
            {
                throw new BadRequestException("The server couldn't process the image.");
            }
        }
    }
}
