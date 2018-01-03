using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Tiveria.Common.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext Context { get; }

        void Save();
        Task SaveAsync(System.Threading.CancellationToken token);
        Task SaveAsync();
        T Repository<T>() where T : class, IRepository;
        IRepository<T> GenericRepository<T>() where T : class, new();
    }
}