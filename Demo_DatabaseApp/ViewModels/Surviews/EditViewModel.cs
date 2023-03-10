// Loginout/Loginout/EditViewModel.cs
// Created by Tiran Spierer
// Created at 08/01/2023
// Class propose:

using System.Linq;
using DataAccess.DataModels;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Prism.Events;

namespace Demo_DatabaseApp.ViewModels.Surviews;

public class EditViewModel : RegisterViewModel
{
    #region Privates

    private string _originalUsername;

    #endregion

    #region Constructors

    public EditViewModel(INavigationService navigationService, IUserService userService, IEventAggregator ea, User user) : base(navigationService, userService, ea)
    {
        _originalUsername = user.Id;

        Name = user.Name;
        Username = user.Id;
        Password = string.Empty;

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

    protected override async void ExecuteRegisterCommandAsync()
    {
        var updatedUser = new User()
        {
            Id = Username!,
            Name = Name,
            Password = Password,
            UserPrivileges = SelectedPrivileges.Select(p => new UserPrivilege()
            {
                Privilege = p,
                UserId = Username!
            }).ToList()
        };

        var isUpdated = await _userService.EditAsync(_originalUsername, updatedUser);

        if (isUpdated == false)
        {
            ErrorMessage = "something went wrong";
        }
        else
        {
            ErrorMessage = "";
            _navigationService.NavigateMainPage(typeof(HomeViewModel));
        }
    }

    #endregion


}