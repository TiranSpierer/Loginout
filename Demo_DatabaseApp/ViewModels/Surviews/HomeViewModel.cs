using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.ViewModels.Subviews;
using Prism.Commands;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class HomeViewModel : ViewModelBase
{
    #region Privates

    private User?                _selectedUser;
    private HomeSubOneViewModel  _homeSubOne;

#endregion

    #region Constructors

    public HomeViewModel(INavigationService navigationService, IUserService userService) : base(navigationService, userService)
    {
        NavigateToLoginCommand    = new DelegateCommand(() => _navigationService.Navigate(typeof(LoginViewModel)));
        NavigateToRegisterCommand = new DelegateCommand(() => _navigationService.Navigate(typeof(RegisterViewModel)));
        NavigateToEditCommand     = new DelegateCommand(ExecuteEdit, CanExecuteEdit);
        RemoveUserCommand         = new DelegateCommand(ExecuteRemoveUser);
        _homeSubOne               = new HomeSubOneViewModel();
        Users                     = new ObservableCollection<User>();
        _                         = InitTable();
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
            NavigateToEditCommand.RaiseCanExecuteChanged();
            HomeSubOne.SelectedUser = SelectedUser;
        }
    }

    public HomeSubOneViewModel HomeSubOne
    {
        get => _homeSubOne;
        set => SetProperty(ref _homeSubOne, value);
    }

#endregion

    #region Private Methods

    private async void ExecuteRemoveUser()
    {
        if (SelectedUser != null)
        {
            var isDeleted = await _userService.DeleteAsync(SelectedUser.Id);
            await InitTable();
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

    private void ExecuteEdit()
    {
        _navigationService.Navigate(typeof(EditViewModel), SelectedUser!);
    }

    private bool CanExecuteEdit() => SelectedUser != null;

    #endregion
}
