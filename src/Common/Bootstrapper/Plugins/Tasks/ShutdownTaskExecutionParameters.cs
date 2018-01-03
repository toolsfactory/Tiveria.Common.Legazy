using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    public class ShutdownTaskExecutionParameters
    {
        public IBootstrapperShutdownTask Task { get; set; }
        public int Position { get; set; }
        public int Delay { get; set; }
        public string Group { get; set; }
    }
}
