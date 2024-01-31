using static Application.Services.FileService;

namespace Application.Interfaces
{
    public partial interface IFileService
    {
        Task<IFileModel> GetFileObjectAsync(Guid Id);
        Task<IDataFileInfoModel> GetFileInfoAsync(Guid Id);
        Task<Guid> SaveFileAsync(IFileModel data);
    }
}
