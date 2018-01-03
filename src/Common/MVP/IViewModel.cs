using System;
using System.ComponentModel;

namespace Tiveria.Common.MVP
{
    public interface IViewModel<TViewModelContract, TViewContract> : INotifyPropertyChanged
        where TViewModelContract : IViewModel<TViewModelContract, TViewContract>
        where TViewContract : IView<TViewContract, TViewModelContract>
    {
        TViewContract View { get; set; }
    }

}