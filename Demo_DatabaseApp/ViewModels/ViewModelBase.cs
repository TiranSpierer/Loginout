
using DataService.Services;
using Loginout.Services;
using Prism.Mvvm;

namespace Loginout.ViewModels;

public class ViewModelBase : BindableBase
{
    protected readonly NavigationService _navigationService;
    protected readonly IUserService _userService;

    public ViewModelBase(NavigationService navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService =       userService;
    }
    
    public virtual void Dispose()    
    {   
    }
}
