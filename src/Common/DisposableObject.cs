using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common
{
    public class DisposableObject : IDisposable
    {
        ~DisposableObject()
        {
            Dispose(false);
        }

        private bool _Disposed = false;
        protected virtual void Dispose(bool includeManagedResources)
        {
            if (!_Disposed)
            {
                if (includeManagedResources)
                {
                    DisposeManagedResources();
                }
                DisposeUnManagedResources();
            }
            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeManagedResources()
        { }

        protected virtual void DisposeUnManagedResources()
        { }
    }
}