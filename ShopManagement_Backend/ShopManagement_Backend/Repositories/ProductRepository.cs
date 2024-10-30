using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;

namespace ShopManagement_Backend.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopManagementDbContext context) : base(context) { }
    }
}
