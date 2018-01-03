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
    public class LightCoreServiceBehavior : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if ((endpointDispatcher.ContractName != "IMetadataExchange") &&
                        (endpointDispatcher.ContractName != "IHttpGetHelpPageAndMetadataContract"))
                    {
                        string contractName = endpointDispatcher.ContractName;
                        ServiceEndpoint serviceEndpoint = serviceDescription.Endpoints.FirstOrDefault(e => e.Contract.Name == contractName);
                        endpointDispatcher.DispatchRuntime.InstanceProvider = new LightCoreInstanceProvider(serviceEndpoint.Contract.ContractType);
                    }
                }
            }
        }
    }
}
