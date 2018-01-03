using System;

namespace Tiveria.Common.WCF
{
    public class LightCoreServiceHostContainer : Tiveria.Common.WCF.ServiceHostContainer
    {
        public LightCoreServiceHostContainer(Type service)
            : base(service)
        {

        }
        public LightCoreServiceHostContainer(Type service, Uri uri)
            : base(service, uri)
        {

        }
        public override void Start(bool fixAddress)
        {
            if (_Uri == null)
                _Host = new LightCoreServiceHost(_Service);
            else
                _Host = new LightCoreServiceHost(_Service, _Uri);

            ((LightCoreServiceHost)_Host).ApplyServiceConfiguration();

            if (fixAddress)
                CheckServiceEndpoint(_Host);

            _Host.Open();
        }
    }
}
