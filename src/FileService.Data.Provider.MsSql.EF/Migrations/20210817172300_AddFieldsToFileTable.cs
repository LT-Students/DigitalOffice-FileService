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
                newName: "CreatedAtUtc"
                );

            migrationBuilder.AddColumn<DateTime>(
               name: "ModifiedAtUtc",
               table: DbFile.TableName,
               nullable: true);

            migrationBuilder.AddColumn<Guid>(
               name: "CreatedBy",
               table: DbFile.TableName,
               nullable: false);

            migrationBuilder.AddColumn<Guid>(
               name: "ModifiedBy",
               table: DbFile.TableName,
               nullable: true);

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: DbFile.TableName);
        }
    }
}
