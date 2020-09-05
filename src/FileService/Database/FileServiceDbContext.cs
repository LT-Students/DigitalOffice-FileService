using LT.DigitalOffice.FileService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Database
{
    /// <summary>
    /// A class that defines the tables and its properties in the database of FileService.
    /// </summary>
    public class FileServiceDbContext : DbContext
    {
        public FileServiceDbContext(DbContextOptions<FileServiceDbContext> options)
            :base(options)
        {
        }

        public DbSet<DbFile> Files { get; set; }

        // Fluent API is written here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
