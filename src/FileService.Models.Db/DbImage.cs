﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.FileService.Models.Db
{
    public class DbImage
    {
        public const string TableName = "Images";

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public DateTime AddedOn { get; set; }
        public Guid UserId { get; set; }
        public int ImageType { get; set; }
        public bool IsActive { get; set; }
    }

    public class DbImageConfiguration : IEntityTypeConfiguration<DbImage>
    {
        public void Configure(EntityTypeBuilder<DbImage> builder)
        {
            builder
                .ToTable(DbImage.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Content)
                .IsRequired();

            builder
                .Property(p => p.Extension)
                .IsRequired();
        }
    }
}
