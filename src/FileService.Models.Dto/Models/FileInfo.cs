namespace LT.DigitalOffice.FileService.Models.Dto.Models
{
  public record FileInfo
  {
    public string Path { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
  }
}
