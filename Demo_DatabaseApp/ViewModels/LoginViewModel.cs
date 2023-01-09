﻿using DataService.Services;
using Loginout.Services;
using Prism.Commands;
using System.Threading.Tasks;

namespace Loginout.ViewModels;

public class LoginViewModel : ViewModelBase
{
    #region Privates

    private string? _password;
    private string? _username;
    private bool _canExecuteLoginCommand;
    private string? _errorMessage;

    #endregion

    #region Constructors

    public LoginViewModel(INavigationService navigationService, IUserService userService) : base(navigationService, userService)
    {
        _password = string.Empty;
        NavigateToHomeCommand = new DelegateCommand(ExecuteLoginCommandAsync).ObservesCanExecute(() => CanExecuteLoginCommand);
    }

    #endregion

    #region Public Properties

    public DelegateCommand NavigateToHomeCommand { get; }

    public bool CanExecuteLoginCommand
    {
        get => _canExecuteLoginCommand;
        set => SetProperty(ref _canExecuteLoginCommand, value);
    }

    public string? Password
    {
        get => _password;
        set
        {
            SetProperty(ref _password, value);
            Task.Run(CanExecuteLoginCommandAsync);
        }
    }

    public string? Username
    {
        get => _username;
        set
        {
            SetProperty(ref _username, value);
            Task.Run(CanExecuteLoginCommandAsync);
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    #endregion

    #region Private Methods

    private void ExecuteLoginCommandAsync()
    {
        _navigationService.Navigate(typeof(HomeViewModel));
    }

    private async Task CanExecuteLoginCommandAsync()
    {
        if(string.IsNullOrEmpty(Username) == false)
        {
            ErrorMessage = "";
            CanExecuteLoginCommand = await _userService.AuthenticateAsync(Username, Password!);
        }
        else
        {
            ErrorMessage = "Wrong username or password";
            CanExecuteLoginCommand = false;
        }
    }

    #endregion

}