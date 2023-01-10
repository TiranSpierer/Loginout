using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Mvvm;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class ViewModelBase : BindableBase
{
    protected readonly INavigationService _navigationService;
    protected readonly IUserService _userService;

    public ViewModelBase(INavigationService navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
    }

    public virtual void Dispose()
    {
    }
}
