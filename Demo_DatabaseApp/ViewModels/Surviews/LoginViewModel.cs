using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Commands;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class LoginViewModel : ViewModelBase
{
    #region Privates

    private string? _password;
    private bool _canExecuteLoginCommand;
    private User? _selectedUser;

    #endregion

    #region Constructors

    public LoginViewModel(INavigationService navigationService, IUserService userService) : base(navigationService, userService)
    {
        _password = string.Empty;
        NavigateToHomeCommand = new DelegateCommand(ExecuteLoginCommandAsync).ObservesCanExecute(() => CanExecuteLoginCommand);
        UsersList = new ObservableCollection<User>();
        InitUsersList();
    }

    #endregion

    #region Public Properties

    public ObservableCollection<User> UsersList { get; set; }

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

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            SetProperty(ref _selectedUser, value);
            Password = string.Empty;
            Task.Run(CanExecuteLoginCommandAsync);
        }
    }

    #endregion

    #region Private Methods

    private void ExecuteLoginCommandAsync()
    {
        _navigationService.Navigate(typeof(HomeViewModel));
    }

    private async Task CanExecuteLoginCommandAsync()
    {
        CanExecuteLoginCommand = await _userService.AuthenticateAsync(SelectedUser?.Id!, Password!);
    }

    private async void InitUsersList()
    {
        UsersList.Clear();
        var freshUsers = await _userService.GetAllUsersAsync();

        var enumerable = freshUsers as User[] ?? freshUsers.ToArray();
        foreach (var user in enumerable)
        {
            UsersList.Add(user);
        }

        SelectedUser = enumerable.FirstOrDefault();
    }

    #endregion

}