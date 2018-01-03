using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Update
{
    class UpdateConfiguration
    {
        public String   ApplicationName     { get; private set; }
        public String   InstalledVersion    { get; private set; }
        public Boolean  CheckForUpdate      { get; private set; }
        public DateTime LastCheckTime       { get; private set; }
        public String   SkipThisVersion     { get; private set; }
        public Boolean  DidRunOnce           { get; private set; }
        public Boolean ShowDiagnosticWindow { get; private set; }
        public DateTime LastProfileUpdate   { get; private set; }    
    }
}
