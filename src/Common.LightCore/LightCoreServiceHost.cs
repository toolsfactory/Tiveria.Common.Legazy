using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using LightCore;

namespace Tiveria.Common.WCF
{
    public class LightCoreServiceHost : ServiceHost
    {

        protected LightCoreServiceHost()
            : base()
        { }
        public LightCoreServiceHost(Type serviceType)
            : base(serviceType)
        {
            ServiceType = serviceType;
        }
        public LightCoreServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            ServiceType = serviceType;
        }
        public LightCoreServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
            ServiceType = singletonInstance.GetType();
        }

        public Type ServiceType { get; private set; }

        public void AddBaseAddress(Uri baseAddress)
        {
            base.AddBaseAddress(baseAddress);
        }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (this.Description.Behaviors.Find<LightCoreServiceBehavior>() == null)
            {
                this.Description.Behaviors.Add(new LightCoreServiceBehavior());
            }
        }

        public virtual void ApplyServiceConfiguration()
        {
            var method = ServiceType.GetMethod("ConfigureService");
            if (method == null || !method.IsStatic)
                return;

            var parameters = method.GetParameters();
            if (parameters == null || parameters.Count() != 1)
                return;

            if (parameters[0].ParameterType != typeof(ServiceHost))
                return;

            method.Invoke(null, new object[] { this });
        }
    }

    public class LightCoreServiceHost<T> : LightCoreServiceHost where T : class
    {
        public LightCoreServiceHost() : base(typeof(T))
        {
        }

        public LightCoreServiceHost(params Uri[] baseAddresses)
            : base(typeof(T), baseAddresses)
        {
            
        }

        private LightCoreServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
            
        }

    }
}
