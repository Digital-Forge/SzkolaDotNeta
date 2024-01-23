namespace Application.Interfaces
{
    public partial interface IFileService
    {
        interface IDataFileInfoModel
        {
            Guid Id { get; set; }
            string Name { get; set; }
            DateTime Date { get; set; }
            bool IsMissing { get; set; }
        }

        interface IFileModel
        {
            IDataFileInfoModel Info { get; set; }
            string DataBase64 { get; set; }
        }
    }
}
