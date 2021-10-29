using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(FileServiceDbContext))]
  [Migration("20211029033600_RemoveIsActive")]
  public class RemoveIsActive : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: "IsActive",
        table: DbFile.TableName);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
        name: "IsActive",
        table: DbFile.TableName,
        nullable: false);
    }
  }
}
