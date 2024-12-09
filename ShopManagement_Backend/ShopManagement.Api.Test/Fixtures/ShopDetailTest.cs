using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Models.Product;

namespace ShopManagement.Api.Test.Fixtures
{
    public class ShopDetailTest
    {
        public PaginationResponse ShopPagination = new PaginationResponse
        {
            PageNumber = 1,
            PageSize = 10,
            TotalOfPages = 1,
            TotalOfNumberRecord = 1,
            Results = new List<ShopDetailResponse>
            {
                new ShopDetailResponse
                {
                    Product = new ProductResponse
                    {
                        ProductId = 1,
                        ProductName = "Áo Adidas",
                        Price = 200
                    },
                    Quantity = 1,
                },
            },
        };
    }
}
