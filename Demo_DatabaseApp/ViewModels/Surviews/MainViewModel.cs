using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;
using Demo_DatabaseApp.ViewModels.Interfaces;
using EventAggregator.ViewModelChanged;
using Prism.Events;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore<MainViewModelChanged>  _navigationStore;

    public INavigableViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore<MainViewModelChanged> navigationStore, INavigationService navigationService, IUserService userService, IEventAggregator ea) : base(navigationService, userService, ea)
    {
        _navigationStore = navigationStore;

        _ea.GetEvent<MainViewModelChanged>().Subscribe(OnCurrentViewModelChanged);
    }

    private void OnCurrentViewModelChanged()
    {
        RaisePropertyChanged(nameof(CurrentViewModel));
    }
}