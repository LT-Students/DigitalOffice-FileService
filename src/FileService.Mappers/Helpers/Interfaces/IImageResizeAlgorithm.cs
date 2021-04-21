namespace LT.DigitalOffice.FileService.Mappers.Helpers.Interfaces
{
    public interface IImageResizeAlgorithm
    {
        public string Resize(string base64String, string outputExtension);
    }
}
