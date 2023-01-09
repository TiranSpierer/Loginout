using Loginout.Services;
using Loginout  .Stores;
using Loginout.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Loginout.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider)
    {
        _navigationStore = navigationStore;
        _serviceProvider = serviceProvider;
    }

    public void Navigate(Type viewModelType)
    {
        var viewModel = ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType) as ViewModelBase;
        Navigate(viewModel);
    }

    public void Navigate(Type viewModelType, params object[] arguments)
    {
        var viewModel = ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, arguments) as ViewModelBase;
        Navigate(viewModel);
    }

    private void Navigate(ViewModelBase? viewModel)
    {
        if (viewModel != null)
        {
            _navigationStore.CurrentViewModel = viewModel;
        }
    }

}


