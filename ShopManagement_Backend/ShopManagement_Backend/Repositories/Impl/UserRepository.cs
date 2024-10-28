using ShopManagement_Backend.Models;

namespace ShopManagement_Backend.Repositories.Impl
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ShopManagementDbContext context) : base(context) { }
    }
}
