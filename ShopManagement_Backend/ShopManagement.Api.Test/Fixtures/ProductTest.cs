using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Core.Entities;
using System.Collections;

namespace ShopManagement.Api.Test.Fixtures
{
    public class ProductTest : IEnumerable<object[]>
    {
        public List<ProductResponse> Products { get; set; } = new List<ProductResponse>
        {
            new ProductResponse
            {
                ProductId = 1,
                ProductName = "Áo Adidas",
                Price = 200,
                ImageUrl = "link1",
            },
            new ProductResponse
            {
                ProductId = 2,
                ProductName = "Argentina 2022",
                Price = 500,
                ImageUrl = "link2",
            }
        };

        public List<Product> productsBeforeMap = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Áo Adidas",
                Price = 200,
                ImageUrl = "link1",
                IsDeleted = false,
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Argentina 2022",
                Price = 500,
                ImageUrl = "link2",
                IsDeleted = false,
            },
        };

        public Product product = new Product
        {
            ProductId = 1,
            ProductName = "Test",
            Price = 200,
            ImageUrl = "url",
            IsDeleted = false,
        };

        public Product productNull = null;

        public ProductResponse productMap = new ProductResponse
        {
            ProductId = 1,
            ProductName = "Test",
            Price = 200,
            ImageUrl = "url",
        };

        public ProductPaginationRequest productRequest = new ProductPaginationRequest
        {
            PageIndex = 1,
            PageSize = 10,
            SearchText = "abs",
            Column = "testColumn",
            Sort = "sortClause",
        };

        public ProductRequest request = new ProductRequest
        {
            ProductName = "Test",
            Price = 200,
            ImageUrl = "url"
        };

        public PaginationResponse ProductPagination = new PaginationResponse
        {
            PageNumber = 1,
            PageSize = 10,
            TotalOfPages = 1,
            TotalOfNumberRecord = 2,
            Results = new List<ProductResponse>
            {
                new ProductResponse
                {
                    ProductId = 1,
                    ProductName = "Áo Adidas",
                    Price = 200
                },
                new ProductResponse
                {
                    ProductId = 2,
                    ProductName = "Argentina 2022",
                    Price = 500
                }
            },
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
