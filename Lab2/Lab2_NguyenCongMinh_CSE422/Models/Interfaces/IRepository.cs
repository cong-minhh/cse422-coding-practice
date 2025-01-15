using System.Linq.Expressions;

namespace Lab2_NguyenCongMinh_CSE422.Models.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets entities based on a predicate
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets a single entity by its ID
        /// </summary>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Removes an entity
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        Task SaveChangesAsync();
    }
}
