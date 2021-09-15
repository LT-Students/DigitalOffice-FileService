using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(FileServiceDbContext))]
    [Migration("20210817172300_AddFieldsToFileTable")]
    class AddFieldsToFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddedOn",
                table: DbFile.TableName,
                newName: nameof(DbFile.CreatedAtUtc));

            migrationBuilder.AddColumn<DateTime>(
               name: nameof(DbFile.ModifiedAtUtc),
               table: DbFile.TableName,
               nullable: true);

            migrationBuilder.AddColumn<Guid>(
               name: nameof(DbFile.CreatedBy),
               table: DbFile.TableName,
               nullable: false);

            migrationBuilder.AddColumn<Guid>(
               name: nameof(DbFile.ModifiedBy),
               table: DbFile.TableName,
               nullable: true);

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: DbFile.TableName);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: nameof(DbFile.CreatedAtUtc),
                table: DbFile.TableName,
                newName: "AddedOn");

            migrationBuilder.DropColumn(
               name: nameof(DbFile.ModifiedAtUtc),
               table: DbFile.TableName);

            migrationBuilder.DropColumn(
               name: nameof(DbFile.CreatedBy),
               table: DbFile.TableName);

            migrationBuilder.DropColumn(
               name: nameof(DbFile.ModifiedBy),
               table: DbFile.TableName);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: DbFile.TableName,
                nullable: false);
        }
    }
}
