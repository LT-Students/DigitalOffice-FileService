using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.FileService.Models.Db
{
  public class DbFile
  {
    public const string TableName = "Files";
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public string Path { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
  }

  public class FileTableConfiguration : IEntityTypeConfiguration<DbFile>
  {
    public void Configure(EntityTypeBuilder<DbFile> builder)
    {
      builder
        .ToTable(DbFile.TableName);

      builder
        .HasKey(p => p.Id);
    }
  }
}
