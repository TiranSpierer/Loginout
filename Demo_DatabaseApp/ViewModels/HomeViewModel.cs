using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Commands;

namespace Demo_DatabaseApp.ViewModels;

public class HomeViewModel : ViewModelBase
{
    #region Privates

    private User? _selectedUser;

#endregion

    #region Constructors

    public HomeViewModel(INavigationService navigationService, IUserService userService) : base(navigationService, userService)
    {
        NavigateToLoginCommand = new DelegateCommand(() => _navigationService.Navigate(typeof(LoginViewModel)));
        NavigateToRegisterCommand = new DelegateCommand(() => _navigationService.Navigate(typeof(RegisterViewModel)));
        NavigateToEditCommand = new DelegateCommand(ExecuteEdit, CanExecuteEdit);
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
            NavigateToEditCommand.RaiseCanExecuteChanged();
        }
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
