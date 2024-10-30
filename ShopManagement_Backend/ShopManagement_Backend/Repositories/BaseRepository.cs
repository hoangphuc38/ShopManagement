using Microsoft.EntityFrameworkCore;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ShopManagement_Backend.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ShopManagementDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected BaseRepository(ShopManagementDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public TEntity AddAsync(TEntity entity)
        {
            var addedEntity = DbSet.Add(entity).Entity;
            Context.SaveChanges();

            return addedEntity;
        }

        public TEntity DeleteAsync(TEntity entity)
        {
            var removedEntity = DbSet.Remove(entity).Entity;
            Context.SaveChanges();

            return removedEntity;
        }

        public List<TEntity> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public TEntity GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            return DbSet.Where(predicate).FirstOrDefault();
        }

        public TEntity? GetFirstOrNullAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }

            return DbSet.Where(predicate).FirstOrDefault();
        }

        public TEntity UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            Context.SaveChanges();

            return entity;
        }
    }
}
