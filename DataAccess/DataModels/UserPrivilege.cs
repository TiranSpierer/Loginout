// UserManager/Domain/UserPrivilege.cs
// Created by Tiran Spierer
// Created at 27/12/2022
// Class propose:

using Domain.Models;

namespace DataAccess.DataModels;

public class UserPrivilege : IEntity<UserPrivilege>
{

    #region Public Properties

    public string    UserId    { get; set; }
    public Privilege Privilege { get; set; }

    #endregion

#region Implementation of IEntity<in UserPrivilege>

    public void CopyValuesTo(UserPrivilege entity)
    {
        entity.Privilege = Privilege;
    }

#endregion
}