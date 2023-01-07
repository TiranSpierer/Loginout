using DataService.Services;
using Loginout.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loginout.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    #region Privates

    private readonly NavigationService<HomeViewModel> _homeNavigationService;
    private readonly IUserService _userService;
    private string? _password;
    private string? _username;
    private string? _name;
    private bool _canExecuteLoginCommand;
    private string? _errorMessage;
    private bool _canExecuteRegisterCommand;

    #endregion

    #region Constructors

    public RegisterViewModel(NavigationService<HomeViewModel> navigationService, IUserService userService)
    {
        _homeNavigationService = navigationService;
        _userService = userService;
        _password = string.Empty;
        RegisterCommand = new DelegateCommand(ExecuteRegisterCommandAsync).ObservesCanExecute(() => CanExecuteRegisterCommand);
        CancelCommand = new DelegateCommand(() => _homeNavigationService.Navigate());
    }

    #endregion

    #region Public Properties

    public DelegateCommand RegisterCommand { get; }
    public DelegateCommand CancelCommand { get; }

    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string? Username
    {
        get => _username;
        set
        {
            SetProperty(ref _username, value);
            CanExecuteRegisterCommand = string.IsNullOrEmpty(Username) == false;

        }
    }

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }


    public bool CanExecuteRegisterCommand
    {
        get => _canExecuteRegisterCommand;
        set => SetProperty(ref _canExecuteRegisterCommand, value);
    }

    #endregion

    #region Private Methods

    private async void ExecuteRegisterCommandAsync()
    {
        var isRegistered = await _userService.RegisterAsync(Username!, Password!, Name!);

        if(isRegistered == false)
        {
            ErrorMessage = "Username taken";
        }
        else
        {
            ErrorMessage = "";
            _homeNavigationService.Navigate();
        }
    }

    #endregion


}
