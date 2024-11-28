using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ShopManagement_Backend_DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ShopManagementDbContext Context;
        protected readonly ILogger<BaseRepository<TEntity>> _logger;
        protected readonly DbSet<TEntity> DbSet;

        protected BaseRepository(
            ShopManagementDbContext context,
            ILogger<BaseRepository<TEntity>> logger)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
            _logger = logger;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation("[AddAsync] Start to connect to db"); 
                var addedEntity = (await DbSet.AddAsync(entity)).Entity;
                await Context.SaveChangesAsync();

                return addedEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AddAsync] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            try
            {
                _logger.LogInformation("[DeleteAsync] Start to connect to db");
                var removedEntity = DbSet.Remove(entity).Entity;
                await Context.SaveChangesAsync();

                return removedEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteAsync] Error: {ex.Message}");
                throw new Exception(ex.Message);
            } 
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await DbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllAsync] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = await DbSet.Where(predicate).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new KeyNotFoundException();
                }

                return await DbSet.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetFirstAsync] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }       
        }

        public async Task<TEntity?> GetFirstOrNullAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = await DbSet.Where(predicate).FirstOrDefaultAsync();

                if (entity == null)
                {
                    return null;
                }

                return await DbSet.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetFirstOrNullAsync] Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            try
            {
                DbSet.Update(entity);
                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetFirstOrNullAsync] Error: {ex.Message}");

                return null;
            }     
        }
    }
}
