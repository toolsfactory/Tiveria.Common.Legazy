using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Tiveria.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <example>
    /// var customers =
    ///            unitOfWork.Repository<Customer>()
    ///                .Query()
    ///                .Include(i => i.CustomerDemographics)
    ///                .OrderBy(q => q
    ///                    .OrderBy(c => c.ContactName)
    ///                    .ThenBy(c => c.CompanyName))
    ///                .Filter(q => q.ContactName != null)
    ///                .GetPage(pageNumber, pageSize, out totalCustomerCount);
    /// </example>

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly Guid _instanceId;
        private IDbSet<TEntity> _dbSet;
        private IDbContext _Context;

         public Repository()
        {
            _instanceId = Guid.NewGuid();
        }

       public Repository(IDbContext context)
           : this()
        {
            Initialize(context);
        }

       public void Initialize(IDbContext context)
       {
           _Context = context;
           _dbSet = context.Set<TEntity>();
       }

       public void ChangeObjectState(object entity, ObjectState state)
       {
           _Context.ChangeObjectState(entity, state);
       }

       public void Attach(TEntity entity)
       {
        if (!_Context.IsAttached(entity))
        {
            _dbSet.Attach(entity);
        }
       }


        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet;
        }

        public TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }


        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void DeleteAll(IEnumerable<TEntity> entity)
        {
            foreach (var ent in entity)
            {
                _dbSet.Remove(ent);
            }
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Attach(entity);
            _Context.ChangeObjectState(entity, ObjectState.Modified);
        }

        public System.Threading.Tasks.Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbSet.Where(whereCondition).FirstOrDefaultAsync<TEntity>();
        }

        public System.Threading.Tasks.Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbSet.Where(whereCondition).ToListAsync<TEntity>();
        }

        public System.Threading.Tasks.Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync<TEntity>();
        }

        public System.Threading.Tasks.Task<long> CountAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbSet.Where(whereCondition).LongCountAsync<TEntity>();
        }

        public System.Threading.Tasks.Task<long> CountAsync()
        {
            return _dbSet.LongCountAsync<TEntity>();
        }
    }
}
