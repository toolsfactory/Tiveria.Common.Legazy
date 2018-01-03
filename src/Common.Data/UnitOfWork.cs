using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiveria.Common.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext, new()
    {
        #region Private Fields
        private Dictionary<Type, IRepository> _repositories;
        private Dictionary<Type, object> _genericRepositories;
        private bool _disposed;
        private readonly Guid _instanceId;
        #endregion Private Fields

        #region Constuctor/Dispose

        public UnitOfWork()
            : this ( new TContext())
        {
        }

        public UnitOfWork(IDbContext context)
        {
            Context = context;
            _instanceId = Guid.NewGuid();
            _repositories = new Dictionary<Type, IRepository>();
            _genericRepositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }
        #endregion Constuctor/Dispose

        public IDbContext Context { get; private set; }

        public Guid InstanceId { get { return _instanceId; } }

        public void Save()
        {
            Context.SaveChanges();
        }

        public Task SaveAsync(System.Threading.CancellationToken token)
        {
            return Context.SaveChangesAsync(token);

        }

        public Task SaveAsync()
        {
            return Context.SaveChangesAsync();

        }

        public T Repository<T>() where T : class, IRepository
        {

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repo = RepositoryResolver.ResolveRepository<T>();
                repo.Initialize(this.Context);

                _repositories.Add(type, repo);
            }

            return (T)_repositories[type];
        }

        public IRepository<T> GenericRepository<T>() where T : class, new()
        {
            // Checks if the Dictionary Key contains the Model class
            if (_genericRepositories.Keys.Contains(typeof(T)))
            {
                // Return the repository for that Model class
                return _genericRepositories[typeof(T)] as IRepository<T>;
            }

            // If the repository for that Model class doesn't exist, create it
            var repository = new Repository<T>(Context);

            // Add it to the dictionary
            _genericRepositories.Add(typeof(T), repository);

            return repository;
        }
    }
}