using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models
{
    public class PaginationResponse
    {
        public PaginationResponse() { }

        public PaginationResponse(int pageNumber, int pageSize, object result)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalOfNumberRecord = 0;
            TotalOfPages = 1;
            Results = result;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalOfNumberRecord { get; set; }

        public int TotalOfPages { get; set; }

        public object? Results { get; set; }
    }
}
