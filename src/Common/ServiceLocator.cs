using System.Collections.Generic;

public static class ServiceLocator
{
    private static ServiceLocatorProvider currentProvider;
    private static IServiceLocator Current
    {
        get
        {
            return ServiceLocator.currentProvider();
        }
    }
    public static void SetLocatorProvider(ServiceLocatorProvider newProvider)
    {
        ServiceLocator.currentProvider = newProvider;
    }

    public static bool HasRegistration<TContract>()
    {
        return Current.HasRegistration<TContract>();
    }

    public static TContract Resolve<TContract>()
    {
        return Current.Resolve<TContract>();
    }

    public static TContract Resolve<TContract>(params object[] arguments)
    {
        return Current.Resolve<TContract>(arguments);
    }

    public static IEnumerable<TContract> ResolveAll<TContract>()
    {
        return Current.ResolveAll<TContract>();
    }

    public static IEnumerable<object> ResolveAll()
    {
        return Current.ResolveAll();
    }

}

public delegate IServiceLocator ServiceLocatorProvider();

public interface IServiceLocator
{
    bool HasRegistration<TContract>();
    TContract Resolve<TContract>();
    TContract Resolve<TContract>(params object[] arguments);
    IEnumerable<TContract> ResolveAll<TContract>();
    IEnumerable<object> ResolveAll();
}
