using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper;
using Tiveria.Common.Bootstrapper.Core;

public static class Booty
{
    private static BootstrapperCore _BootstrapperCore = null;
    private static readonly object _LockObject = new object();

    public static IBootstrapperContainer Container 
    { 
        get
        {
            if (_BootstrapperCore == null)
                throw new InvalidOperationException("Cannot access Bootstrapper prior to calling it's Create method");

            return _BootstrapperCore.Container;
        }
    }

    public static IBootstrapperConfiguration Create()
    {
        lock(_LockObject)
        {
            if(_BootstrapperCore != null)
                throw new InvalidOperationException("Cannot Create Bootstrapper more than once");

            _BootstrapperCore = new BootstrapperCore();
            return _BootstrapperCore;
        }
    }

    public static void Shutdown()
    {
        lock (_LockObject)
        {
            if(_BootstrapperCore != null)
                _BootstrapperCore.Shutdown();
            _BootstrapperCore = null;
        }
    }

    public static bool Started
    {
        get
        {
            if (_BootstrapperCore == null)
                return false;
            else
                return _BootstrapperCore.Started;
        }
    }

    public static bool Stopped
    {
        get
        {
            if (_BootstrapperCore == null)
                return false;
            else
                return _BootstrapperCore.Stopped;
        }
    }

}
