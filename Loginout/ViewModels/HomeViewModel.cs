using DataAccess.DataModels;
using DataService.Services;
using Loginout.Services;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Loginout.ViewModels;

public class HomeViewModel : ViewModelBase
{
    #region Privates

    private readonly NavigationService<LoginViewModel>    _loginNavigationService;
    private readonly NavigationService<RegisterViewModel> _registerNavigationService;
    private readonly NavigationService<EditViewModel>     _editNavigationService;
    private readonly IUserService                         _userService;
    private          User?                                _selectedUser;

#endregion

    #region Constructors

    public HomeViewModel(NavigationService<LoginViewModel> loginNavigationService, NavigationService<RegisterViewModel> registerNavigationService, NavigationService<EditViewModel> editNavigationService, IUserService userService)
    {
        _loginNavigationService    = loginNavigationService;
        _registerNavigationService = registerNavigationService;
        _editNavigationService     = editNavigationService;
        _userService               = userService;

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
        set => SetProperty(ref _selectedUser, value);
    }
    #endregion

    #region Private Methods

    private void ExecuteNavigateToEdit()
    {
        _editNavigationService.Navigate();
    }

    private async void ExecuteRemoveUser()
    {
        if (SelectedUser != null)
        {
            await _userService.DeleteAsync(SelectedUser.Id);
        }
    }

    private async Task InitTable()
    {
        Users.Clear();

        var freshUsers = await _userService.GetAllUsersAsync();
        foreach (var user in freshUsers)
        {
            Users.Add(user);
        }
    }

    #endregion
}
