using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(FileServiceDbContext))]
  [Migration("20220813011023_InitialCreate")]
  public class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@"
        CREATE TABLE [dbo].[Files]
        AS FILETABLE
        WITH (FILETABLE_DIRECTORY = N'Files')");
    }
  }
}
