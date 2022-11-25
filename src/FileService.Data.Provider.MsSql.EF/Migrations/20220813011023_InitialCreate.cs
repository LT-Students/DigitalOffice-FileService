using System;
using LT.DigitalOffice.FileService.Models.Dto.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(FileServiceDbContext))]
  [Migration("20220813011023_InitialCreate")]
  public class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      CreateTable(builder, DBTablesNames.Wiki);
      CreateTable(builder, DBTablesNames.PROJECT);
    }

    private static void CreateTable(MigrationBuilder builder, string tableName)
    {
      builder.CreateTable(
        name: tableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false, maxLength: 245),
          Extension = table.Column<string>(nullable: true, maxLength: 10),
          Size = table.Column<long>(nullable: false),
          Path = table.Column<string>(nullable: false, maxLength: 300),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
          ModifiedBy = table.Column<Guid>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{tableName}", x => x.Id);
        });
    }
  }
}
