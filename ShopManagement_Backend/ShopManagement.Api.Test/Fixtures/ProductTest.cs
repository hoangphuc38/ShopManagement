using ShopManagement_Backend_Application.Models.Product;
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

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
