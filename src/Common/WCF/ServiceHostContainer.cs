using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Tiveria.Common.WCF
{
    public class ServiceHostContainer : IDisposable, IServiceHostContainer
    {

        protected Type _Service;
        protected System.ServiceModel.ServiceHost _Host;
        protected Uri _Uri;

        public System.ServiceModel.Description.ServiceDescription Description { get { if(_Host == null)  return null; return _Host.Description; } }
        public string ServiceName { get { if (_Service == null) return String.Empty; return _Service.Name; } }

        public ServiceHostContainer(Type service)
        {
            _Service = service;
            _Uri = null;
        }

        public ServiceHostContainer(Type service, Uri uri)
        {
            _Service = service;
            _Uri = uri;
        }

        public void Dispose()
        {
            if (_Host != null)
                _Host.Abort();
            _Host = null;
        }

        public virtual void Start(bool fixAddress)
        {
            if (_Uri == null)
                _Host = new System.ServiceModel.ServiceHost(_Service);
            else
                _Host = new System.ServiceModel.ServiceHost(_Service, _Uri);
            
            if (fixAddress)
                CheckServiceEndpoint(_Host);

            _Host.Open();
        }

        public virtual void Close()
        { 
            if (_Host != null)
                _Host.Close(); 
        }

        public virtual void Abort()
        {
            _Host.Abort();
        }
        protected void CheckServiceEndpoint(System.ServiceModel.ServiceHost host)
        {
            if (host.Description.Endpoints.Count < 1)
                throw new ArgumentException("No Endpoints");

            if (host.Description.Endpoints[0].Address.Uri.IsLoopback)
            {
                var currentUri = host.Description.Endpoints[0].Address.Uri;
                host.Description.Endpoints[0].Address = new System.ServiceModel.EndpointAddress(new Uri(String.Format("{0}://{1}:{2}{3}", currentUri.Scheme, GetLocalIP(), currentUri.Port, currentUri.PathAndQuery)));
            }
        }

        private IPAddress GetLocalIP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in localIPs)
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.GetAddressBytes()[0] != 169 && ip.GetAddressBytes()[1] != 254)

                    return ip;
            return IPAddress.None;
        }
    }
}