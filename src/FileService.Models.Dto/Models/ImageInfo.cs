using LT.DigitalOffice.FileService.Models.Dto.Enums;
using System;

namespace LT.DigitalOffice.FileService.Models.Dto.Models
{
    public record ImageInfo
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public ImageType Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
    }
}
