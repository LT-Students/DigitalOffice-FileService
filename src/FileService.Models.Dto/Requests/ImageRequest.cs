﻿using System;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.FileService.Models.Dto.Requests
{
    public class ImageRequest
    {
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}