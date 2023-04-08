using System.Linq.Expressions;

namespace Micro_House_Manage_API.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
