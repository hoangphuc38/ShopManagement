using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Helpers
{
    public class PaginationHelper<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Pagination(IEnumerable<TEntity> entityList, int page, int pageSize)
        {
            return entityList.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
