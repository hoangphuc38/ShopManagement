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
                Price = 200
            },
            new ProductResponse
            {
                ProductId = 2,
                ProductName = "Argentina 2022",
                Price = 500
            }
        };

        public List<Product> ProductList { get; set; } = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                ProductName = "Argentina 2022",
                Price = 200,
                IsDeleted = false,
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Brazil 2022",
                Price = 200,
                IsDeleted = false,
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
