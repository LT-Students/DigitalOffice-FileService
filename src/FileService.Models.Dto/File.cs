using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.FileService.Models.Dto
{
    public class File
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }
}
