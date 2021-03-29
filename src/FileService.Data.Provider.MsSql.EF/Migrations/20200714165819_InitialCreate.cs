using System;
using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(FileServiceDbContext))]
    [Migration("20200714165819_InitialCreate")]
    public class InitialCreate : Migration
    {
        #region Create tables

        private void CreateTableFiles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbFile.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<byte[]>(nullable: false),
                    Extension = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ClosedAt = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });
        }

        private void CreateTableImages(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbImage.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Content = table.Column<byte[]>(nullable: false),
                    Extension = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ImageType = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        #endregion

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateTableFiles(migrationBuilder);

            CreateTableImages(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(DbFile.TableName);

            migrationBuilder.DropTable(DbImage.TableName);
        }
    }
}
