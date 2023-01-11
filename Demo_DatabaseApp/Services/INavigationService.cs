using System;

namespace Demo_DatabaseApp.Services;

public interface INavigationService
{
    void NavigateMainPage(Type viewModelType, params object[]? arguments);
    void NavigateSubPage(Type  viewModelType, params object[]? arguments);
}
