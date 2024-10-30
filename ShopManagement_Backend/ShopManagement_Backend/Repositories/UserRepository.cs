using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;

namespace ShopManagement_Backend.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ShopManagementDbContext context) : base(context) { }
    }
}
