using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Events;
using Prism.Mvvm;

namespace Demo_DatabaseApp.ViewModels.Interfaces;

public class ViewModelBase : BindableBase, INavigableViewModel
{
    protected readonly INavigationService _navigationService;
    protected readonly IUserService       _userService;
    protected readonly   IEventAggregator   _ea;

    public ViewModelBase(INavigationService navigationService, IUserService userService, IEventAggregator ea)
    {
        _navigationService = navigationService;
        _userService       = userService;
        _ea           = ea;
    }

    public virtual void Dispose()
    {
    }
}
