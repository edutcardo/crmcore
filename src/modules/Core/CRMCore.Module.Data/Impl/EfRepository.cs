using CRMCore.Module.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CRMCore.Module.Data.Impl
{
    public class EfRepositoryAsync<TEntity> 
        : EfRepositoryAsync<ApplicationDbContext, TEntity>, IEfRepositoryAsync<TEntity>
        where TEntity : class, IEntity
    {
        public EfRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EfQueryRepository<TEntity> 
        : EfQueryRepository<ApplicationDbContext, TEntity>, IEfQueryRepository<TEntity>
        where TEntity : class, IEntity
    {
        public EfQueryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EfRepositoryAsync<TDbContext, TEntity> : IEfRepositoryAsync<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        private readonly TDbContext _dbContext;

        public EfRepositoryAsync(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var dbEntityEntry = _dbContext.Entry(entity);
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
    }

    public class EfQueryRepository<TDbContext, TEntity> : IEfQueryRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        private readonly TDbContext _dbContext;

        public EfQueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryFluent<TEntity, TResponse> Return<TResponse>(IQueryObject<TEntity> queryObject)
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>, queryObject);

        public IQueryFluent<TEntity, TResponse> Return<TResponse>(Expression<Func<TEntity, bool>> query)
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>, query);

        public IQueryFluent<TEntity, TResponse> Return<TResponse>() 
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>);

        public IQueryable<TEntity> Queryable() => _dbContext.Set<TEntity>();
    }
}
