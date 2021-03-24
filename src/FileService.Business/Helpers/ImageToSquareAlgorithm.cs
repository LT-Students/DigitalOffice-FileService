using LT.DigitalOffice.FileService.Business.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
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
namespace LT.DigitalOffice.FileService.Business.Helpers
{
    internal class ImageToSquareAlgorithm : IImageResizeAlgorithm
    {
        private static readonly Dictionary<string, IImageFormat> formats = new Dictionary<string, IImageFormat>
        {
            { ".jpg", JpegFormat.Instance },
            { ".jpeg", JpegFormat.Instance },
            { ".png", PngFormat.Instance },
            { ".bmp", BmpFormat.Instance },
            { ".gif", GifFormat.Instance },
            { ".tga", TgaFormat.Instance }
        };

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
