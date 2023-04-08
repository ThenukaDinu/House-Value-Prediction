using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Micro_House_Manage_API.Repository
{
    public class BaseRepository <TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _dataContext;

        public BaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dataContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dataContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dataContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dataContext.Set<TEntity>().Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dataContext.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dataContext.Set<TEntity>().AnyAsync(predicate);
        }
    }
}
