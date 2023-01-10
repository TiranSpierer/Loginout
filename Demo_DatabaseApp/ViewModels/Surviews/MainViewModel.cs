using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;

    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore navigationStore, INavigationService navigationService, IUserService userService) : base(navigationService, userService)
    {
        _navigationStore = navigationStore;

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        RaisePropertyChanged(nameof(CurrentViewModel));
    }
}