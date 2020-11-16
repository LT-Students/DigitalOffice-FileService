﻿using LT.DigitalOffice.FileService.Mappers.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.FileService.Models.Dto;
using LT.DigitalOffice.FileService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.FileService.Mappers
{
    public class FileMapper : IMapper<DbFile, File>, IMapper<File, DbFile>
    {
        public File Map(DbFile dbFile)
        {
            if (dbFile == null)
            {
                throw new ArgumentNullException(nameof(dbFile));
            }

            return new File()
            {
                Id = dbFile.Id,
                Content = Convert.ToBase64String(dbFile.Content),
                Extension = dbFile.Extension,
                Name = dbFile.Name
            };
        }

        public DbFile Map(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return new DbFile()
            {
                Content = Convert.FromBase64String(file.Content),
                Extension = file.Extension,
                Name = file.Name,
                AddedOn = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}