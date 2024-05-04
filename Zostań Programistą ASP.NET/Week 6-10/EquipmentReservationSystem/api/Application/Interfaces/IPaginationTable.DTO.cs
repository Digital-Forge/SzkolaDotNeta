using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public partial interface IPaginationTable<T> where T : class
    {
        public class TableOptions
        {
            public int? Take { get; set; }
            public int? Skip { get; set; }
            public string? Search { get; set; }
        }

        public class TableData
        {
            public long TotalCount { get; set; }
            public List<T> Data { get; set; }
        }
    }
}
