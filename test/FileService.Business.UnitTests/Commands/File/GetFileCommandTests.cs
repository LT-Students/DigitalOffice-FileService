using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.FileService.Business.Commands.Files;
using LT.DigitalOffice.FileService.Business.Commands.Files.Interfaces;
using LT.DigitalOffice.FileService.Data.Interfaces;
using LT.DigitalOffice.FileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.FileService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.FileService.Business.UnitTests.Commands.File
{
  public class GetFileCommandTests
  {
    /*private AutoMocker _autoMocker;
    private IGetFileCommand _command;
    private List<DbFile> _dbFiles;
    private List<FileInfo> _infoFile;
    List<Guid> _ids = new() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
      _autoMocker = new AutoMocker();
      _command = _autoMocker.CreateInstance<GetFilesCommand>();

      _dbFiles = new List<DbFile>()
      {
        new DbFile()
        {
          Id = _ids[0],
          Content = "Test1",
          Extension = "Test1",
          Name = "Test1",
          CreatedAtUtc = DateTime.UtcNow,
          CreatedBy = Guid.NewGuid(),
        },

        new DbFile()
        {
          Id = _ids[1],
          Content = "Test2",
          Extension = "Test2",
          Name = "Test2",
          CreatedAtUtc = DateTime.UtcNow,
          CreatedBy = Guid.NewGuid(),
        },

        new DbFile()
        {
          Id = _ids[2],
          Content = "Test3",
          Extension = "Test3",
          Name = "Test3",
          CreatedAtUtc = DateTime.UtcNow,
          CreatedBy = Guid.NewGuid(),
        }
      };

      _infoFile = new List<FileInfo>();

      foreach (var file in _dbFiles)
      {
        _infoFile.Add(
          new FileInfo()
          {
            Id = file.Id,
            Content = file.Content,
            Extension = file.Extension,
            Name = file.Name
          });
      }
    }

    [SetUp]
    public void SetUp()
    {
      _autoMocker.GetMock<IFileRepository>().Reset();
      _autoMocker.GetMock<IFileInfoMapper>().Reset();
    }

    [Test]
    public async Task ShouldReturnListFileInfo()
    {
      var result = _infoFile;

      _autoMocker
        .Setup<IFileRepository, Task<List<DbFile>>>(x => x.GetAsync(It.IsAny<List<Guid>>()))
        .ReturnsAsync(_dbFiles);

      _autoMocker
        .SetupSequence<IFileInfoMapper, FileInfo>(x => x.Map(It.IsAny<DbFile>()))
        .Returns(_infoFile[0])
        .Returns(_infoFile[1])
        .Returns(_infoFile[2]);

      SerializerAssert.AreEqual(result, (await _command.ExecuteAsync(_ids)).Body);

      _autoMocker.Verify<IFileRepository, Task<List<DbFile>>>(x => x.GetAsync(It.IsAny<List<Guid>>()), Times.Once);
      _autoMocker.Verify<IFileInfoMapper, FileInfo>(x => x.Map(It.IsAny<DbFile>()), Times.Exactly(3));
    }

    [Test]
    public async Task ShouldReturnNullInRepository()
    {
      List<DbFile> dblist = null;
      List<Guid> ids = new() { Guid.NewGuid() };

      _autoMocker
        .Setup<IFileRepository, Task<List<DbFile>>>(x => x.GetAsync(It.IsAny<List<Guid>>()))
        .ReturnsAsync(dblist);

      SerializerAssert.AreEqual(null, (await _command.ExecuteAsync(ids)).Body);

      _autoMocker.Verify<IFileRepository, Task<List<DbFile>>>(x => x.GetAsync(It.IsAny<List<Guid>>()), Times.Once);
      _autoMocker.Verify<IFileInfoMapper, FileInfo>(x => x.Map(It.IsAny<DbFile>()), Times.Never);
    }

    [Test]
    public async Task ShouldReturnNull()
    {
      _ids = null;

      SerializerAssert.AreEqual(null, await _command.ExecuteAsync(_ids));

      _autoMocker.Verify<IFileRepository, Task<List<DbFile>>>(x => x.GetAsync(It.IsAny<List<Guid>>()), Times.Never);
      _autoMocker.Verify<IFileInfoMapper, FileInfo>(x => x.Map(It.IsAny<DbFile>()), Times.Never);
    }*/
  }
}
