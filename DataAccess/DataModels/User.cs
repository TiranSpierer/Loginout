using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataModels;

public class User : IEntity<User>
{
    [Key]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }

    //public ICollection<UserPrivilege>? UserPrivileges { get; set; }
    //public virtual ICollection<Procedure> Procedures { get; set; }

    #region Implementation of IEntity<in User>
    public void CopyValuesTo(User entity)
    {
        entity.Id = Id;
        entity.Name = Name;
        entity.Password = Password;
        //entity.UserPrivileges = UserPrivileges;
    }

    #endregion
}
