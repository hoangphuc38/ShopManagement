using System.Linq.Expressions;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetFirstOrNullAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> UpdateAsync(TEntity entity);
    }
}
