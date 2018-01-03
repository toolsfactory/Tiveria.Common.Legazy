using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.DependencyInjection
{
    public class SimpleContainer
    {
        private readonly IDictionary<Type, ServiceDescriptor> _Services = new Dictionary<Type, ServiceDescriptor>();

        public void Register(Type t, object instance)
        {
            _Services.Add(new KeyValuePair<Type, ServiceDescriptor>(t, new ServiceDescriptor() { Instance = instance, ServiceType = t }));
        }

        public TService Get<TService>()
        {
            return (TService)Get(typeof(TService));
        }

        public object Get(Type tService)
        {
            return GetInstance(tService);
        }

        public void ForEach<TService>(Action<TService> action)
        {
            foreach (var service in _Services)
            {
                if (service.Value.Instance is TService)
                    action((TService)service.Value.Instance);
            }
        }

        public void ForEach(Action<object> action)
        {
            foreach (var service in _Services)
            {
                    action(service.Value.Instance);
            }
        }

        private object GetInstance(Type tService)
        {
            if (_Services.ContainsKey(tService))
                return _Services[tService].Instance;

            throw new Exception("Type not registered" + tService);
        }

        internal class ServiceDescriptor
        {
            public Type ServiceType { get; set; }
            public object Instance { get; set; }
        }
    }
}
