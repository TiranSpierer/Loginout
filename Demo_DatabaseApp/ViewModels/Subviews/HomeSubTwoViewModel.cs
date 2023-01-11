// Created by Tiran Spierer
// Created at 10/01/2023
// Class purpose:


using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.ViewModels.Interfaces;
using Prism.Commands;

namespace Demo_DatabaseApp.ViewModels.Subviews;

public class HomeSubTwoViewModel : INavigableViewModel
{
    private readonly INavigationService _navigationService;

#region Privates



    #endregion

    #region Constructors

    public HomeSubTwoViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        NextSubviewCommand = new DelegateCommand(ExecuteNextSubview);
    }


#endregion

    #region Public Properties

    public DelegateCommand NextSubviewCommand { get; }

    #endregion

    #region Public Methods

    private void ExecuteNextSubview()
    {
        _navigationService.NavigateSubPage(typeof(HomeSubOneViewModel));
    }

    #endregion

    #region Private Methods


    #endregion

    #region Implementation of INavigableViewModel

    public void Dispose()
    {
        
    }

#endregion
}