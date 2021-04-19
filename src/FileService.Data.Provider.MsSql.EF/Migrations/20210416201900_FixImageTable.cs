using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(FileServiceDbContext))]
    [Migration("20210416201900_FixImageTable")]
    public class FixImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: DbImage.TableName);

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: DbImage.TableName,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: DbImage.TableName);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageType",
                table: DbImage.TableName,
                nullable: false);
        }
    }
}
