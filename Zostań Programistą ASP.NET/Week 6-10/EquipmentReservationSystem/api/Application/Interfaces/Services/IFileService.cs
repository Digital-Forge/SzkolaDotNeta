using static Application.Services.FileService;

namespace Application.Interfaces
{
    public partial interface IFileService
    {
        Task<FileModel> GetFileObjectAsync(Guid Id);
        Task<DataFileInfoModel> GetFileInfoAsync(Guid Id);
        Task<Guid> SaveFileAsync(FileModel data);
    }
}
