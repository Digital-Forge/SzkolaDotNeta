using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public partial interface IPaginationTable<T> where T : class
    {
        TableData GetTable(TableOptions options);
        Task<TableData> GetTablAsync(TableOptions options);
    }
}
