using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataModels;

public class User
{
    [Key]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }

    //public ICollection<UserPrivilege>? UserPrivileges { get; set; }
    //public virtual ICollection<Procedure> Procedures { get; set; }
}
