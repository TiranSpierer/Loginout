using DataService.Services;
using Loginout.Services;
using Prism.Commands;
using System.Threading.Tasks;

namespace Loginout.ViewModels;

public class LoginViewModel : ViewModelBase
{
    #region Privates

    private readonly NavigationService<HomeViewModel> _navigationService;
    private readonly IUserService _userService;
    private string? _password;
    private string? _username;
    private bool _canExecuteLoginCommand;

    #endregion

    #region Constructors

    public LoginViewModel(NavigationService<HomeViewModel> navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
        _password = string.Empty;
        LoginCommand = new DelegateCommand(ExecuteLoginCommandAsync).ObservesCanExecute(() => CanExecuteLoginCommand);
    }

    #endregion

    #region Public Properties

    public DelegateCommand LoginCommand { get; }

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

    #endregion

    #region Private Methods

    private void ExecuteLoginCommandAsync()
    {
        _navigationService.Navigate();
    }

    private async Task CanExecuteLoginCommandAsync()
    {
        if(string.IsNullOrEmpty(Username) == false)
        {
            CanExecuteLoginCommand = await _userService.AuthenticateAsync(Username, Password!);
        }
        else
        {
            CanExecuteLoginCommand = false;
        }
    }

    #endregion

}