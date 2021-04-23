using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.FileService.Models.Dto.Requests
{
    public class FileRequest
    {
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }
}
