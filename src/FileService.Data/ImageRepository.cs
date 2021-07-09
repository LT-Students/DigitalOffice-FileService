using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Data.Provider;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.FileService.Data
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDataProvider _provider;

        public ImageRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public Guid Add(DbImage dbImage)
        {
            _provider.Images.Add(dbImage);
            _provider.Save();

            return dbImage.Id;
        }

        public DbImage Get(Guid imageId)
        {
            return _provider.Images.FirstOrDefault(x => x.Id == imageId)
                ?? throw new NotFoundException($"No image with id {imageId}");
        }

        public List<DbImage> Get(List<Guid> imageIds)
        {
            return _provider.Images.Where(x => imageIds.Contains(x.Id)).ToList();
        }
    }
}
