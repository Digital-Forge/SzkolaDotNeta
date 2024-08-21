using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IFileService))]
    public partial class FileService : IFileService
    {
        private readonly string _path;
        private readonly double? _dateCleaningSpan;
        private readonly IFileRepository _fileRepository;
        private readonly IAuthService _authService;

        public bool SystemHostedServiceMode { 
            get => _fileRepository.SystemHostedServiceMode; 
            set => _fileRepository.SystemHostedServiceMode = value;
        }

        public FileService(IFileRepository fileRepository, IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _fileRepository = fileRepository;
            _path = config["File:FilesPath"] ?? "";
            _dateCleaningSpan = double.TryParse(config["File:CleaningSpan"], out double timeValue) ? timeValue : null;
        }

        public async Task<IFileService.DataFileInfoModel> GetFileInfoAsync(Guid Id)
        {
            var entity = await _fileRepository.GetAsync(Id) ?? throw new FileNotFoundException();

            if (entity.EntityStatus == Domain.Utils.EntityStatus.Buffer)
                if (!await _authService.IsUserAdminAsync()) throw new FileBufferStatusException();

            return DataFileInfoModel.Map(entity);
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

        public async Task<Guid> SaveFileAsync(IFileService.FileModel data, bool isTemporary = false)
        {
            var entity = DataFileInfoModel.Map(data.Info);
            entity.EntityStatus = Domain.Utils.EntityStatus.Buffer;
            await _fileRepository.SaveAsync(entity);

            var file = Convert.FromBase64String(data.DataBase64);

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            await File.WriteAllBytesAsync(Path.Combine(_path, entity.Id.ToString()), file);

            if (!isTemporary)
            {
                entity.EntityStatus = Domain.Utils.EntityStatus.Use;
                await _fileRepository.SaveAsync(entity);
            }

            return entity.Id;
        }

        public async Task UpdateFileInfo(IFileService.DataFileInfoModel model, bool isTemporary = false)
        {
            var entity = await _fileRepository.GetAsync(model.Id.Value);
            DataFileInfoModel.Map(model, entity);

            entity.EntityStatus = isTemporary
                ? Domain.Utils.EntityStatus.Buffer
                : Domain.Utils.EntityStatus.Use;

            await _fileRepository.SaveAsync(entity);
        }

        public async Task DeleteFileAsync(Guid Id)
        {
            var entity = _fileRepository.GetAsync(Id);

            var path = Path.Combine(_path, entity?.Id.ToString() ?? throw new FilePathException()) ?? string.Empty;

            if (File.Exists(path))
                File.Delete(path);

            await _fileRepository.RemoveAsync(Id);
        }

        public void DeleteFile(Guid Id)
        {
            var entity = _fileRepository.Get(Id);
            var path = Path.Combine(_path, entity?.Id.ToString() ?? throw new FilePathException()) ?? string.Empty;

            if (File.Exists(path))
                File.Delete(path);

            _fileRepository.Remove(Id);
        }

        public async Task UpdateTemporary(Guid id, bool isTemporary)
        {
            var entity = await _fileRepository.GetAsync(id);

            entity.EntityStatus = isTemporary
                ? Domain.Utils.EntityStatus.Buffer
                : Domain.Utils.EntityStatus.Use;

            await _fileRepository.SaveAsync(entity);
        }

        public void ClearTemporaryFiles()
        {
            if (_dateCleaningSpan == null) return;

            var dateMarginFileter = DateTime.Now.AddDays(-_dateCleaningSpan.Value);

            var files = _fileRepository.GetTemporaryFiles()
                .Where(x => x.UpdateTime < dateMarginFileter)
                .Select(s => s.Id)
                .ToList();

            foreach (var file in files)
            {
                DeleteFile(file);
            }
        }
    }
}
