﻿using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.FileService.Models.Dto.Configurations
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetFileEndpoint { get; set; }
        public string AddImageEndpoint { get; set; }
    }
}