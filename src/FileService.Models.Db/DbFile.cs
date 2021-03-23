﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.FileService.Models.Db
{
    public class DbFile
    {
        public const string TableName = "Files";

        public Guid Id { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class DbFileConfiguration : IEntityTypeConfiguration<DbFile>
    {
        public void Configure(EntityTypeBuilder<DbFile> builder)
        {
            //builder
            //    .ToTable(DbFile.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Extension)
                .IsRequired();
        }
    }
}
