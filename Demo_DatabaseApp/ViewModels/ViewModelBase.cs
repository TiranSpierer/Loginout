
using DataService.Services;
using Loginout.Services;
using Prism.Mvvm;

namespace Loginout.ViewModels;

public class ViewModelBase : BindableBase
{
    protected readonly INavigationService _navigationService;
    protected readonly IUserService _userService;

    public ViewModelBase(INavigationService navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService =       userService;
    }
    
    public virtual void Dispose()    
    {   
    }
}
