using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.File;

namespace LT.DigitalOffice.FileService.Mappers.Models
{
    public class FileDataMapper : IFileDataMapper
    {
        public FileData Map(DbFile dbFile)
        {
            if (dbFile == null)
            {
                return null;
            }

            return new FileData(dbFile.Id, dbFile.Content, dbFile.Extension, dbFile.Name);
        }
    }
}