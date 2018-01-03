using System;
using System.ServiceModel;

namespace Tiveria.Common.WCF
{
    public abstract class ServiceClientBase<TInterface>
    {
        public string EndpointAddress { get; private set; }
        public string EndpointConfiguration { get; private set; }


        public ServiceClientBase()
        { }

        public ServiceClientBase(string endpointConfiguration, string endpointAddress)
        {
            ConfigureEndpoint(endpointConfiguration, endpointAddress);
        }

        public ServiceClientBase(string endpointConfiguration)
            : this(endpointConfiguration, "")
        {
        }

        protected void ConfigureEndpoint(string endpointConfiguration, string endpointAddress = "")
        {
            if (String.IsNullOrWhiteSpace(endpointConfiguration))
                throw new ArgumentNullException("BindingConfiguration must not be empty");
            EndpointConfiguration = endpointConfiguration;
            EndpointAddress = endpointAddress;
        }

        protected ChannelFactory<TInterface> CreateFactory()
        {
            if (String.IsNullOrWhiteSpace(EndpointAddress))
                return new ChannelFactory<TInterface>(EndpointConfiguration);
            else
                return new ChannelFactory<TInterface>(EndpointConfiguration, new EndpointAddress(EndpointAddress));
        }

        protected DuplexChannelFactory<TInterface> CreateDuplexFactory(object callback)
        {
            if (String.IsNullOrWhiteSpace(EndpointAddress))
                return new DuplexChannelFactory<TInterface>(new InstanceContext(callback), EndpointConfiguration);
            else
                return new DuplexChannelFactory<TInterface>(new InstanceContext(callback), EndpointConfiguration, new EndpointAddress(EndpointAddress));
        }

        protected void CloseChannel(TInterface channel)
        {
            try
            {
                if (channel != null)
                    ((ICommunicationObject)channel).Close();
            }
            catch
            {
            }
            finally
            {
                ((ICommunicationObject)channel).Abort();
            }
        }
    }
}
