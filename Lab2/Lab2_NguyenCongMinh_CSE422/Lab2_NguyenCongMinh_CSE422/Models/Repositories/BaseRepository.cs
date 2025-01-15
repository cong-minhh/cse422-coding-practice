using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Lab2_NguyenCongMinh_CSE422.Models.Interfaces;

namespace Lab2_NguyenCongMinh_CSE422.Models.Repositories
{
    /// <summary>
    /// Base implementation of the generic repository pattern
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DeviceManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DeviceManagementContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
