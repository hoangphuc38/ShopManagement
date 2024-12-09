using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopManagement_Backend_Application.Models.Shop;

namespace ShopManagement.Api.Test.Fixtures
{
    public class ShopTest
    {
        public PaginationResponse ShopPagination = new PaginationResponse
        {
            PageNumber = 1,
            PageSize = 10,
            TotalOfPages = 1,
            TotalOfNumberRecord = 2,
            Results = new List<ShopResponse>
            {
                new ShopResponse
                {
                    ShopID = 1,
                    ShopName = "Becamex",
                    ShopAddress = "Binh Duong",
                    CreatedDate = DateTime.Now,
                    OwnerName = "Phuc"
                },
                new ShopResponse
                {
                    ShopID = 2,
                    ShopName = "Becamex",
                    ShopAddress = "Binh Duong",
                    CreatedDate = DateTime.Now,
                    OwnerName = "Phuc"
                },
            },
        };

        public ShopResponse shopRes = new ShopResponse
        {
            ShopID = 1,
            ShopName = "Becamex",
            ShopAddress = "Binh Duong",
            CreatedDate = DateTime.Now,
            OwnerName = "Phuc"
        };
    }
}
