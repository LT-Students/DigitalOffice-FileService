using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using System;

namespace LT.DigitalOffice.FileService.Data
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDataProvider provider;

        public ImageRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public Guid AddImage(DbImage dbImage)
        {
            provider.Images.Add(dbImage);
            provider.Save();

            return dbImage.Id;
        }
    }
}
