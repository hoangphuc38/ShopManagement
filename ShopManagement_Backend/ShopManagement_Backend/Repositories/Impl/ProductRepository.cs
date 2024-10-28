using ShopManagement_Backend.Models;

namespace ShopManagement_Backend.Repositories.Impl
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopManagementDbContext context): base(context) { }
    }
}
