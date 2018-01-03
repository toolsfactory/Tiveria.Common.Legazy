using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Tiveria.Common.Data
{
    public interface IDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        void ChangeObjectState(object entity, ObjectState state);
        int SaveChanges();
        Task<int> SaveChangesAsync(System.Threading.CancellationToken token);
        Task<int> SaveChangesAsync();
        bool IsAttached(object entity);
    }

    public enum ObjectState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }              
}

/*
 *         private static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
*/
