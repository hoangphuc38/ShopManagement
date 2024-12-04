using ShopManagement_Backend_Core.Constants;

namespace ShopManagement_Backend_Application.Models.Shop
{
    public class ShopPaginationRequest
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchText { get; set; }

        public string Column { get; set; } = ColumnName.ShopName;

        public string Sort { get; set; } = SortType.Ascending;
    }
}
