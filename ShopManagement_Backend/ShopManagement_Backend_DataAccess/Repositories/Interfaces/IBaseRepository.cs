using System.Linq.Expressions;

namespace ShopManagement_Backend_DataAccess.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity AddAsync(TEntity entity);

        TEntity DeleteAsync(TEntity entity);

        List<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity? GetFirstOrNullAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity UpdateAsync(TEntity entity);
    }
}
