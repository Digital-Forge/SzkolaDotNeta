using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IFileService))]
    public partial class FileService : IFileService
    {
        private readonly string _path;
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository, IConfiguration config)
        {
            _fileRepository = fileRepository;
            _path = config["FilesPath"] ?? "";
        }

        public async Task<IFileService.DataFileInfoModel> GetFileInfoAsync(Guid Id)
        {
            return DataFileInfoModel.Map(await _fileRepository.GetDataFileAsync(Id) ?? throw new FileNotFoundException());
        }

        public async Task<IFileService.FileModel> GetFileObjectAsync(Guid Id)
        {
            var data = new IFileService.FileModel()
            {
                Info = await GetFileInfoAsync(Id)
            };

            var path = Path.Combine(_path, data.Info.Id.ToString()) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                data.Info.IsMissing = true;
            }
            else
            {
                try
                {
                    var fileData = await File.ReadAllBytesAsync(path);
                    data.DataBase64 = Convert.ToBase64String(fileData);
                }
                catch
                {
                    data.Info.IsMissing = true;
                }
            }
            return data;
        }

        public async Task<Guid> SaveFileAsync(IFileService.FileModel data)
        {
            var entity = DataFileInfoModel.Map(data.Info);
            entity.EntityStatus = Domain.Utils.EntityStatus.Buffer;
            await _fileRepository.SaveDataFileAsync(entity);

            var file = Convert.FromBase64String(data.DataBase64);
            await File.WriteAllBytesAsync(Path.Combine(_path, entity.Id.ToString()), file);
            entity.EntityStatus = Domain.Utils.EntityStatus.Use;
            await _fileRepository.SaveDataFileAsync(entity);

            return entity.Id;
        }
    }
}
