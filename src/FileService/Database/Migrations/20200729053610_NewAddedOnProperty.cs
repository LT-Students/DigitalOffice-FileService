using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Database.Migrations
{
    public partial class NewAddedOnProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentExtension",
                table: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "Files",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "ContentExtension",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
