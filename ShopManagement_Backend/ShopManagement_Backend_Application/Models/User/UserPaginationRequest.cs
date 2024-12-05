using ShopManagement_Backend_Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.User
{
    public class UserPaginationRequest
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchText { get; set; }

        public string Column { get; set; } = ColumnName.FullName;

        public string Sort { get; set; } = SortType.Ascending;
    }
}
