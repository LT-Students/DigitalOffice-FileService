using LT.DigitalOffice.FileService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.FileService.Data.Provider.MsSql.Ef
{
    /// <summary>
    /// A class that defines the tables and its properties in the database of FileService.
    /// </summary>
    public class FileServiceDbContext : DbContext, IDataProvider
    {
        public FileServiceDbContext(DbContextOptions<FileServiceDbContext> options)
            :base(options)
        {
        }

        public DbSet<DbFile> Files { get; set; }

        void IDataProvider.Save()
        {
            this.SaveChanges();
        }

        public void EnsureDeleted()
        {
            this.Database.EnsureDeleted();
        }

        public bool IsInMemory()
        {
            return this.Database.IsInMemory();
        }

    // Fluent API is written here.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
