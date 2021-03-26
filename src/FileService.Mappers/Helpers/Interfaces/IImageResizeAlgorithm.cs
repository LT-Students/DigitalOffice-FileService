namespace LT.DigitalOffice.FileService.Business.Helpers.Interfaces
{
    public interface IImageResizeAlgorithm
    {
        public byte[] Resize(string base64String, string outputExtension);
    }
}
