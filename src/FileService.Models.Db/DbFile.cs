using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.FileService.Models.Db
{
  public class DbFile
  {
    public const string TableName = "Files";

    [Column("stream_id")]
    public Guid Id { get; set; }

    [Column("file_stream")]
    public byte[] FileStream { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("file_type")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FileType { get; set; }

    [Column("cached_file_size")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public long? CachedFileSize { get; set; }

    [Column("creation_time")]
    public DateTimeOffset CreationTime { get; set; }

    [Column("last_write_time")]
    public DateTimeOffset LastWriteTime { get; set; }

    [Column("last_access_time")]
    public DateTimeOffset? LastAccessTime { get; set; }

    [Column("is_directory")]
    public bool IsDirectory { get; set; }

    [Column("is_offline")]
    public bool IsOffline { get; set; }

    [Column("is_hidden")]
    public bool IsHidden { get; set; }

    [Column("is_readonly")]
    public bool IsReadOnly { get; set; }

    [Column("is_archive")]
    public bool IsArchive { get; set; }

    [Column("is_system")]
    public bool IsSystem { get; set; }

    [Column("is_temporary")]
    public bool IsTemporary { get; set; }
  }

  public class FileTableConfiguration : IEntityTypeConfiguration<DbFile>
  {
    public void Configure(EntityTypeBuilder<DbFile> builder)
    {
      builder
        .ToTable(DbFile.TableName);

      builder
        .HasKey(p => p.Id);

      builder
        .Property(P => P.Name)
        .HasMaxLength(255);

      builder
        .Property(P => P.FileType)
        .HasMaxLength(255);
    }
  }
}
