using System;

namespace LT.DigitalOffice.FileService.Models.Dto.Models
{
  public record FileInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
  }
}
