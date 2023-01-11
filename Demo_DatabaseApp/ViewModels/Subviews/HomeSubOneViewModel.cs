// Created by Tiran Spierer
// Created at 10/01/2023
// Class purpose:

using System;
using System.Collections.ObjectModel;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;
using Demo_DatabaseApp.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace Demo_DatabaseApp.ViewModels.Subviews;

public class HomeSubOneViewModel: BindableBase, INavigableViewModel
{
    private readonly INavigationService _navigationService;
    private          User              _selectedUser;

#region Privates



#endregion

#region Constructors

    public HomeSubOneViewModel(INavigationService navigationService, User selectedUser)
    {
        _navigationService = navigationService;
        Procedures         = new ObservableCollection<Procedure>();
        NextSubviewCommand = new DelegateCommand(ExecuteNextSubview);
        UpdateTableCommand = new DelegateCommand(InitProcedures);
        _selectedUser      = selectedUser;
    }

#endregion

    #region Public Properties

    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            InitProcedures();
        } 
    }

    public ObservableCollection<Procedure> Procedures   { get; set; }

    public DelegateCommand NextSubviewCommand { get; }
    public DelegateCommand UpdateTableCommand { get; }

    #endregion

    #region Public Methods

    public void Dispose()
    {

    }


    #endregion

    #region Private Methods

    private void ExecuteNextSubview()
    {
        _navigationService.NavigateSubPage(typeof(HomeSubTwoViewModel));
    }

    private void InitProcedures()
    {
        Procedures.Clear();
        var rand = new Random();
        var procedure1 = new Procedure()
                         {
                             Id        = rand.Next(),
                             PatientId = rand.Next(),
                             UserId    = SelectedUser.Id
        };
        var procedure2 = new Procedure()
                         {
                             Id        = rand.Next(),
                             PatientId = rand.Next(),
                             UserId    = SelectedUser.Id
        };

        Procedures.Add(procedure1);
        Procedures.Add(procedure2);

        //foreach (var procedure in SelectedUser.Procedures)
        //{
        //    Procedures.Add(procedure);
        //}
    }


    #endregion

}