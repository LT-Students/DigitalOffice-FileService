namespace LT.DigitalOffice.FileService.Models.Dto.Requests
{
    public record AddImageRequest
    {
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }
}
