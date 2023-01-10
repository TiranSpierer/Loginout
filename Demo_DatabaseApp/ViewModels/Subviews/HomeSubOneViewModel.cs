// Created by Tiran Spierer
// Created at 10/01/2023
// Class purpose:

using System;
using System.Collections.ObjectModel;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Mvvm;

namespace Demo_DatabaseApp.ViewModels.Subviews;

public class HomeSubOneViewModel
{
    private User? _selectedUser;

#region Privates



#endregion

#region Constructors

    public HomeSubOneViewModel()
    {
        Procedures         = new ObservableCollection<Procedure>();
    }

    #endregion

    #region Public Properties

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            InitProcedures();
        } 
    }

    public ObservableCollection<Procedure> Procedures   { get; set; }

#endregion

#region Public Methods



#endregion

#region Private Methods

    private void InitProcedures()
    {
        Procedures.Clear();
        var rand = new Random();
        var procedure1 = new Procedure()
                         {
                             Id        = rand.Next(),
                             PatientId = rand.Next(),
                             UserId    = SelectedUser!.Id
        };
        var procedure2 = new Procedure()
                         {
                             Id        = rand.Next(),
                             PatientId = rand.Next(),
                             UserId    = SelectedUser!.Id
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