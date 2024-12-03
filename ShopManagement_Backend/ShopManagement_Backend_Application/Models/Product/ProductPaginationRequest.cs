using ShopManagement_Backend_Core.Constants;
using System.ComponentModel;

namespace ShopManagement_Backend_Application.Models.Product
{
    public class ProductPaginationRequest
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchText { get; set; }

        public string Column { get; set; } = ColumnName.ProductID;

        public string Sort { get; set; } = SortType.Ascending;
    }
}
