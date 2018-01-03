using System;

namespace Tiveria.Common.MVP
{
    public interface IPresenter : IDisposable
    { }

    public interface IPresenter<TModel>
        where TModel : class
    {
        TModel Model { get; }
    }

}
