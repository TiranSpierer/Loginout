using System;

namespace Demo_DatabaseApp.Services;

public interface INavigationService
{
    void Navigate(Type viewModelType);
    void Navigate(Type viewModelType, params object[] arguments);
}
