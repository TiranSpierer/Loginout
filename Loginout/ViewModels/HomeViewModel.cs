using DataAccess.DataModels;
using DataService.Services;
using Loginout.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loginout.ViewModels;

public class HomeViewModel : ViewModelBase
{
    #region Privates

    private readonly NavigationService<LoginViewModel> _loginNavigationService;
    private readonly NavigationService<RegisterViewModel> _registerNavigationService;
    private readonly IUserService _userService;
    private User? _selectedUser;

    #endregion

    #region Constructors

    public HomeViewModel(NavigationService<LoginViewModel> loginNavigationService, NavigationService<RegisterViewModel> registerNavigationService, IUserService userService)
    {
        _loginNavigationService = loginNavigationService;
        _registerNavigationService = registerNavigationService;
        _userService = userService;

        NavigateToLoginCommand = new DelegateCommand(() => _loginNavigationService.Navigate());
        NavigateToRegisterCommand = new DelegateCommand(() => _registerNavigationService.Navigate());
        NavigateToEditCommand = new DelegateCommand(ExecuteNavigateToEdit);
        RemoveUserCommand = new DelegateCommand(ExecuteRemoveUser);

        Users = new ObservableCollection<User>();
        _ = InitTable();
    }

    #endregion

    #region Public Properties

    public DelegateCommand NavigateToLoginCommand { get; }
    public DelegateCommand NavigateToRegisterCommand { get; }
    public DelegateCommand NavigateToEditCommand { get; }
    public DelegateCommand RemoveUserCommand { get; }

    public ObservableCollection<User> Users { get; set; }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            SetProperty(ref _selectedUser, value);
            //CanExecuteRemoveCommand = CanExecuteEditCommand = value != null;
        }
    }
    #endregion

    #region Private Methods

    private void ExecuteNavigateToEdit()
    {
        
    }

    private void ExecuteRemoveUser()
    {

    }

    private async Task InitTable()
    {
        Users.Clear();

        var freshUsers = await _userService.GetAllUsers();
        foreach (var user in freshUsers)
        {
            Users.Add(user);
        }
    }

    #endregion
}
