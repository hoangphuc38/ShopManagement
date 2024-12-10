using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Core.Entities;

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

        public Shop shop = new Shop
        {
            ShopId = 1,
            ShopName = "Becamex",
            ShopAddress = "Binh Duong",
            CreatedDate = DateTime.Now,
            IsDeleted = false,
            UserId = 1,
        };

        public Shop shopNull = null;

        public List<Shop> shopList = new List<Shop>
        {
            new Shop
            {
                ShopId = 1,
                ShopName = "Becamex",
                ShopAddress = "Binh Duong",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = 1,
            },
            new Shop
            {
                ShopId = 2,
                ShopName = "Becamex",
                ShopAddress = "Binh Duong",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                UserId = 1,
            }
        };

        public List<ShopResponse> shopListMap = new List<ShopResponse>
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
        };

        public ShopPaginationRequest paginationRequest = new ShopPaginationRequest
        {
            PageIndex = 1,
            PageSize = 10,
            SearchText = "abs",
            Column = "testColumn",
            Sort = "sortClause",
        };

        public ShopRequest request = new ShopRequest
        {
            ShopAddress = "Test",
            ShopName = "Test",
        };
    }
}
