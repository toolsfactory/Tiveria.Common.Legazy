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
    internal class LightCoreInstanceProvider : IInstanceProvider
    {
        private readonly Type contractType;

        public LightCoreInstanceProvider(Type contractType)
        {
            this.contractType = contractType;
        }
 
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Booty.Container.Resolve(contractType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}