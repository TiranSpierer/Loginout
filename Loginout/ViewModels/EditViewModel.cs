// Loginout/Loginout/EditViewModel.cs
// Created by Tiran Spierer
// Created at 08/01/2023
// Class propose:

using System;
using DataAccess.DataModels;
using DataService.Services;
using Domain.Models;
using Loginout.Services;

namespace Loginout.ViewModels;

public class EditViewModel : RegisterViewModel
{
    #region Privates



    #endregion

    #region Constructors

    public EditViewModel(NavigationService<HomeViewModel> navigationService, IUserService userService, User? user = null) : base(navigationService, userService)
    {
        Name               = user?.Name;
        Username           = user?.Name;
        Password           = string.Empty;
        if (user?.UserPrivileges != null)
        {
            foreach (var privilege in user.UserPrivileges)
            {
                SelectedPrivileges.Add(privilege.Privilege);
                switch (privilege.Privilege)
                {
                    case Privilege.AddUsers:
                        IsAddUsersSelected = true;
                        break;
                    case Privilege.EditUsers:
                        IsEditUsersSelected = true;
                        break;
                    case Privilege.DeleteUsers:
                        IsDeleteUsersSelected = true;
                        break;
                }
            }
        }
        
    }

    #endregion

    #region Public Properties



    #endregion

    #region Public Methods



    #endregion

    #region Private Methods



    #endregion
    

}