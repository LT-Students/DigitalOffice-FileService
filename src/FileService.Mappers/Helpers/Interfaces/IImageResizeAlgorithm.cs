namespace LT.DigitalOffice.FileService.Business.Helpers.Interfaces
{
    public interface IImageResizeAlgorithm
    {
        public string Resize(string base64String, string outputExtension);
    }
}
