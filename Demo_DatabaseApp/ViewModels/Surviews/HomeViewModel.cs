using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;
using Demo_DatabaseApp.ViewModels.Interfaces;
using Demo_DatabaseApp.ViewModels.Subviews;
using EventAggregator.ViewModelChanged;
using Prism.Commands;
using Prism.Events;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class HomeViewModel : ViewModelBase
{

#region Privates

    private readonly NavigationStore<SubViewModelChanged> _navigationStore;
    private          User?                                _selectedUser;

#endregion

    #region Constructors

    public HomeViewModel(INavigationService navigationService, NavigationStore<SubViewModelChanged> navigationStore, IUserService userService, IEventAggregator ea) : base(navigationService, userService, ea)
    {
        _navigationStore          = navigationStore;

        NavigateToLoginCommand    = new DelegateCommand(() => _navigationService.NavigateMainPage(typeof(LoginViewModel)));
        NavigateToRegisterCommand = new DelegateCommand(() => _navigationService.NavigateMainPage(typeof(RegisterViewModel)));
        NavigateToEditCommand     = new DelegateCommand(ExecuteEdit, CanExecuteEdit);
        RemoveUserCommand         = new DelegateCommand(ExecuteRemoveUser);

        _ea.GetEvent<SubViewModelChanged>().Subscribe(() => RaisePropertyChanged(nameof(CurrentSubViewModel)));
        navigationService.NavigateSubPage(typeof(HomeSubOneViewModel));

        Users = new ObservableCollection<User>();
        _     = InitTable();
    }

    #endregion

    #region Public Properties

    public INavigableViewModel CurrentSubViewModel => _navigationStore.CurrentViewModel;


    public DelegateCommand     NavigateToLoginCommand    { get; }
    public DelegateCommand     NavigateToRegisterCommand { get; }
    public DelegateCommand     NavigateToEditCommand     { get; }
    public DelegateCommand     RemoveUserCommand         { get; }

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
        _navigationService.NavigateMainPage(typeof(EditViewModel), SelectedUser!);
    }

    private bool CanExecuteEdit() => SelectedUser != null;

#endregion
}
