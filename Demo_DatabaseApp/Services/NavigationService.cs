using System;
using Demo_DatabaseApp.Stores;
using Demo_DatabaseApp.ViewModels.Interfaces;
using Demo_DatabaseApp.ViewModels.Surviews;
using EventAggregator.ViewModelChanged;
using Microsoft.Extensions.DependencyInjection;

namespace Demo_DatabaseApp.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationStore<MainViewModelChanged> _mainNavigationStore;
    private readonly NavigationStore<SubViewModelChanged>  _subNavigationStore;
    private readonly IServiceProvider                      _serviceProvider;

    public NavigationService(NavigationStore<MainViewModelChanged> mainNavigationStore, NavigationStore<SubViewModelChanged> subNavigationStore, IServiceProvider serviceProvider)
    {
        _mainNavigationStore = mainNavigationStore;
        _subNavigationStore  = subNavigationStore;
        _serviceProvider     = serviceProvider;
    }

    public void NavigateMainPage(Type viewModelType, object[]? arguments = null)
    {
        Navigate(viewModelType, arguments, _mainNavigationStore);
    }

    public void NavigateSubPage(Type viewModelType, object[]? arguments = null)
    {
        Navigate(viewModelType, arguments, _subNavigationStore);
    }

    private void Navigate(Type viewModelType, object[]? arguments, INavigationStore navigationStore)
    {
        var viewModel = (arguments == null)
                            ? ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType)
                            : ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, arguments);

        if (viewModel is INavigableViewModel vm)
        {
            navigationStore.CurrentViewModel = vm;
        }
    }

}
