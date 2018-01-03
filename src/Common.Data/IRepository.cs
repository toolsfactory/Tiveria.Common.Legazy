using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tiveria.Common.Data
{
    public interface IRepository
    {
        void Initialize(IDbContext context);
    }

    /// <summary>
    /// A generic interface for a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepository
          where T : class
    {
        void ChangeObjectState(object entity, ObjectState state);
        IQueryable<T> AsQueryable();

        T FindById(object id);

        /// <summary> 
        /// Add entity to the repository 
        /// </summary> 
        /// <param name="entity">the entity to add</param> 
        /// <returns>The added entity</returns> 
        T Add(T entity);

        /// <summary> 
        /// Mark entity to be deleted within the repository 
        /// </summary> 
        /// <param name="entity">The entity to delete</param> 
        void Delete(T entity);
        void Delete(object id);
        void DeleteAll(IEnumerable<T> entity);

        /// <summary> 
        /// Updates entity within the the repository 
        /// </summary> 
        /// <param name="entity">the entity to update</param> 
        /// <returns>The updates entity</returns> 
        void Update(T entity);


        void Attach(T entity);

        /// <summary>
        /// Get a selected extiry by the object primary key ID
        /// </summary>
        /// <param name="id">Primary key ID</param>
        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition);

        /// <summary> 
        /// Load the entities using a linq expression filter
        /// </summary> 
        /// <typeparam name="E">the entity type to load</typeparam> 
        /// <param name="where">where condition</param> 
        /// <returns>the loaded entity</returns> 
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// Get all the element of this repository
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();
        
        /// <summary>
        /// Count using a filer
        /// </summary>
        Task<long> CountAsync(Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// All item count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<long> CountAsync();

    } 
   
}