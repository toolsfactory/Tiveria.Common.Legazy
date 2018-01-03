using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper;
using Tiveria.Common.Bootstrapper.Core;
using LightCore;

namespace Tiveria.Common.Bootstrapper
{
    class BootstrapperLightCorePlugin : Plugins.IBootstrapperContainerPlugin
    {
        private IContainerBuilder _Builder;
        private IContainer _Container;

        public BootstrapperLightCorePlugin(IContainerBuilder builder)
        {
            _Builder = builder;
        }

        public void Initialize(Core.IBootstrapperContext context)
        {
            context.AssembliesConfiguration.ExcludeAssembly("LightCore");
        }

        public void Startup(IBootstrapperContext context)
        {
            var instances = context.GetInstancesOfTypesImplementing<IBootstrapperLightCoreRegistration>();
            instances.ForEach(a => a.Register(_Builder, context.Bag));

            _Container = _Builder.Build();
        }

        public void Shutdown(IBootstrapperContext context)
        {
        }

        public bool HasRegistration<TContract>()
        {
            return _Container.HasRegistration<TContract>();
        }

        public bool HasRegistration(Type contractType)
        {
            return _Container.HasRegistration(contractType);
        }

        public TContract Resolve<TContract>()
        {
            return _Container.Resolve<TContract>();
        }

        public object Resolve(Type contractType)
        {
            return _Container.Resolve(contractType);
        }

        public IEnumerable<TContract> ResolveAll<TContract>()
        {
            return _Container.ResolveAll<TContract>();
        }

        public IEnumerable<object> ResolveAll(Type contractType)
        {
            return _Container.ResolveAll(contractType);
        }

        public IEnumerable<object> ResolveAll()
        {
            return _Container.ResolveAll();
        }
    }

}