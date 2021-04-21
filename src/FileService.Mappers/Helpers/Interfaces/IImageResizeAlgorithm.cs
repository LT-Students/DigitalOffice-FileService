using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.FileService.Mappers.Helpers.Interfaces
{
    [AutoInject]
    public interface IImageResizeAlgorithm
    {
        public string Resize(string base64String, string outputExtension);
    }
}
