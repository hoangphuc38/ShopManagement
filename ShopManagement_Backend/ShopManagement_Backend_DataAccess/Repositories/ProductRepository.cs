using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopManagementDbContext context) : base(context) { }
    }
}
