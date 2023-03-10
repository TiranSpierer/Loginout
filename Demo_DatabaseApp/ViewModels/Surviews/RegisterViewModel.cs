using System.Collections.Generic;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class RegisterViewModel : ViewModelBase
{
    #region Privates

    protected string? _password;
    protected string? _username;
    protected string? _name;
    protected string? _errorMessage;
    protected bool _canExecuteRegisterCommand;
    protected bool _isAddUsersSelected;
    protected bool _isDeleteUsersSelected;
    protected bool _isEditUsersSelected;

    #endregion

    #region Constructors

    public RegisterViewModel(INavigationService navigationService, IUserService userService, IEventAggregator ea) : base(navigationService, userService, ea)
    {
        _password = string.Empty;
        SelectedPrivileges = new HashSet<Privilege>();

        RegisterCommand = new DelegateCommand(ExecuteRegisterCommandAsync).ObservesCanExecute(() => CanExecuteRegisterCommand);
        CancelCommand = new DelegateCommand(() => _navigationService.NavigateMainPage(typeof(HomeViewModel)));
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

    protected void UpdatePrivilegesList(Privilege privilege, bool isSelected)
    {

        if (isSelected)
            SelectedPrivileges.Add(privilege);
        else
            SelectedPrivileges.Remove(privilege);
    }

    protected virtual async void ExecuteRegisterCommandAsync()
    {
        var isRegistered = await _userService.RegisterAsync(Username!, Password!, Name!, SelectedPrivileges);

        if (isRegistered == false)
        {
            ErrorMessage = "Username taken";
        }
        else
        {
            ErrorMessage = "";
            _navigationService.NavigateMainPage(typeof(HomeViewModel));
        }
    }

    #endregion


}
