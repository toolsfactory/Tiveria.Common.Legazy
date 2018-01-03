using LightCore;

namespace Tiveria.Common.LightCore
{
    class ServiceLocatorAdapter : IServiceLocator
    {
        private readonly IContainer _Container;

        public ServiceLocatorAdapter(IContainer container)
        {
            this._Container = container;
        }

        public bool HasRegistration<TContract>()
        {
            return _Container.HasRegistration<TContract>();
        }

        public TContract Resolve<TContract>()
        {
            return _Container.Resolve<TContract>();
        }

        public TContract Resolve<TContract>(params object[] arguments)
        {
            return _Container.Resolve<TContract>(arguments);
        }

        public System.Collections.Generic.IEnumerable<TContract> ResolveAll<TContract>()
        {
            return _Container.ResolveAll<TContract>();
        }

        public System.Collections.Generic.IEnumerable<object> ResolveAll()
        {
            return _Container.ResolveAll();
        }

        public object GetService(System.Type serviceType)
        {
            return _Container.Resolve(serviceType);
        }
    }
}
