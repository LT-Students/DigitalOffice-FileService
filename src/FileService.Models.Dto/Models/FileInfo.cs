using System;

namespace LT.DigitalOffice.FileService.Models.Dto.Models
{
    public record FileInfo
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }
}
