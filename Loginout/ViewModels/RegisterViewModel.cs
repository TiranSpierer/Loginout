using DataService.Services;
using Domain.Models;
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
    private string? _errorMessage;
    private bool _canExecuteRegisterCommand;
    private bool _isAddUsersSelected;
    private bool _isDeleteUsersSelected;
    private bool _isEditUsersSelected;

    #endregion

    #region Constructors

    public RegisterViewModel(NavigationService<HomeViewModel> navigationService, IUserService userService)
    {
        _homeNavigationService = navigationService;
        _userService = userService;

        _password = string.Empty;
        SelectedPrivileges = new HashSet<Privilege>();

        RegisterCommand = new DelegateCommand(ExecuteRegisterCommandAsync).ObservesCanExecute(() => CanExecuteRegisterCommand);
        CancelCommand = new DelegateCommand(() => _homeNavigationService.Navigate());
    }

    #endregion

    #region Public Properties

    public DelegateCommand RegisterCommand { get; }
    public DelegateCommand CancelCommand { get; }
    public HashSet<Privilege> SelectedPrivileges { get; set; }

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

    public bool IsAddUsersSelected
    {
        get => _isAddUsersSelected;
        set
        {
            SetProperty(ref _isAddUsersSelected, value);
            UpdatePrivilegesList(Privilege.AddUsers, value);
        }
    }

    public bool IsDeleteUsersSelected
    {
        get => _isDeleteUsersSelected;
        set
        {
            SetProperty(ref _isDeleteUsersSelected, value);
            UpdatePrivilegesList(Privilege.DeleteUsers, value);
        }
    }

    public bool IsEditUsersSelected
    {
        get => _isEditUsersSelected;
        set
        {
            SetProperty(ref _isEditUsersSelected, value);
            UpdatePrivilegesList(Privilege.EditUsers, value);
        }
    }

    #endregion

    #region Private Methods

    private void UpdatePrivilegesList(Privilege privilege, bool isSelected)
    {

        if (isSelected)
            SelectedPrivileges.Add(privilege);
        else
            SelectedPrivileges.Remove(privilege);
    }

    private async void ExecuteRegisterCommandAsync()
    {
        var isRegistered = await _userService.RegisterAsync(Username!, Password!, Name!, SelectedPrivileges);

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
