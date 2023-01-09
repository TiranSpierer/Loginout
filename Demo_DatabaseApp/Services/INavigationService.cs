using System;

namespace Loginout.Services;

public interface INavigationService
{
    void Navigate(Type viewModelType);
    void Navigate(Type viewModelType, params object[] arguments);
}
