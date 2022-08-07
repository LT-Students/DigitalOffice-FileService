using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using MimeTypes;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
  public class ContentTypeMapper : IContentTypeMapper
  {
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public ContentTypeMapper()
    {
      _fileExtensionContentTypeProvider =  new FileExtensionContentTypeProvider();
    }

    public string Map(string extension)
    {
      if (extension is null)
      {
        return null;
      }

      if (!MimeTypeMap.TryGetMimeType(extension, out string contentType))
      {
        contentType = "application/octet-stream";
      }

      return contentType;
    }
  }
}
