namespace Application.Interfaces
{
    public partial interface IFileService
    {
        class DataFileInfoModel
        {
            public Guid? Id { get; set; }
            public bool Active { get; set; }
            public string Name { get; set; }
            public DateTime? Date { get; set; }
            public bool? IsMissing { get; set; }
        }

        class FileModel
        {
            public DataFileInfoModel Info { get; set; }
            public string DataBase64 { get; set; }
        }
    }
}
