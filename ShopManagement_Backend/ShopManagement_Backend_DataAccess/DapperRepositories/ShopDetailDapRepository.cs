using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;
using ShopManagement_Backend_DataAccess.Persistance;

namespace ShopManagement_Backend_DataAccess.DapperRepositories
{
    public class ShopDetailDapRepository : IShopDetailDapRepository
    {
        protected readonly ShopManagementDapperContext Context;

        public ShopDetailDapRepository(ShopManagementDapperContext context)
        {
            Context = context;
        }

        public int AddAsync(ShopDetail entity)
        {
            throw new NotImplementedException();
        }

        public int DeleteAsync(ShopDetail entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ShopDetail> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public ShopDetail? GetFirstOrNullAsync(object param)
        {
            throw new NotImplementedException();
        }

        public int UpdateAsync(ShopDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
