namespace ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces
{
    public interface IBaseDapRepository<TEntity> where TEntity : class
    {
        int AddAsync(TEntity entity);

        int DeleteAsync(TEntity entity);

        int UpdateAsync(TEntity entity);

        IEnumerable<TEntity> GetAllAsync();

        TEntity? GetFirstOrNullAsync(object param);
    }
}
