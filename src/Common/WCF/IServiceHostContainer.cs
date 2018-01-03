using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Tiveria.Common.WCF
{
    public interface IServiceHostContainer
    {
        System.ServiceModel.Description.ServiceDescription Description { get; }
        string ServiceName { get; }
        void Start(bool fixAddress);
        void Close();
        void Abort();
    }
}
