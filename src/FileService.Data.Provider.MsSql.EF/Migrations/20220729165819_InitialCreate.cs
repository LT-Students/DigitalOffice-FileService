using System;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(FileServiceDbContext))]
  [Migration("20220729165819_InitialCreate")]
  public class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbFile.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Content = table.Column<string>(nullable: false),
          Extension = table.Column<string>(nullable: false),
          Name = table.Column<string>(nullable: true),
          Size = table.Column<long>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
          ModifiedBy = table.Column<Guid>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Files", x => x.Id);
        });
    }
  }
}
