namespace Application.Interfaces
{
    public partial interface IFileService
    {
        Task<FileModel> GetFileObjectAsync(Guid Id);
        Task<DataFileInfoModel> GetFileInfoAsync(Guid Id);
        Task<Guid> SaveFileAsync(FileModel data, bool isTemporary = false);
        Task UpdateFileInfo(DataFileInfoModel model, bool isTemporary = false);
        Task UpdateTemporary(Guid id ,bool isTemporary);
        Task DeleteFileAsync(Guid Id);
    }
}
