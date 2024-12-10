using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Core.Entities;

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

        public List<ShopDetail> shopDetailList = new List<ShopDetail>
        {
            new ShopDetail
            {
                ShopId = 1,
                ProductId = 1,
                Quantity = 10,
                IsDeleted = false,
            },
            new ShopDetail
            {
                ShopId = 1,
                ProductId = 2,
                Quantity = 10,
                IsDeleted = false,
            },
        };

        public List<ShopDetailResponse> shopDetailListMap = new List<ShopDetailResponse>
        {
            new ShopDetailResponse
            {
                Product = new ProductResponse
                {
                    ProductId = 1,
                    ProductName = "Áo Adidas",
                    Price = 200
                },
                Quantity = 10,
            },
            new ShopDetailResponse
            {
                Product = new ProductResponse
                {
                    ProductId = 2,
                    ProductName = "Áo Adidas",
                    Price = 200
                },
                Quantity = 10,
            },
        };

        public ShopDetail shopDetail = new ShopDetail
        {
            ShopId = 1,
            ProductId = 2,
            Quantity = 10,
            IsDeleted = false,
        };

        public ShopDetail shopDetailNull = null;

        public ShopDetailRequest request = new ShopDetailRequest
        {
            ShopId = 1,
            ProductId = 2,
            Quantity = 10,
        };

        public ShopDetailPaginationRequest paginationRequest = new ShopDetailPaginationRequest
        {
            PageIndex = 1,
            PageSize = 10,
            SearchText = "abs",
            Column = "testColumn",
            Sort = "sortClause",
        };
    }
}
