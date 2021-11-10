using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.FileService.Models.Db
{
  public class DbFile
  {
    public const string TableName = "Files";

    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
  }

  public class DbFileConfiguration : IEntityTypeConfiguration<DbFile>
  {
    public void Configure(EntityTypeBuilder<DbFile> builder)
    {
      builder
        .ToTable(DbFile.TableName);

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
