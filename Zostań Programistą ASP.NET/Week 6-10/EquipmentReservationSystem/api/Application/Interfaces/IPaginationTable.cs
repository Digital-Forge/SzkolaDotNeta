namespace Application.Interfaces
{
    public partial interface IPaginationTable<T> where T : class
    {
        TableData GetTable(TableOptions options);
        Task<TableData> GetTablAsync(TableOptions options);
    }
}
