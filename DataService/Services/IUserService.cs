using DataAccess.DataModels;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataService.Services;

public interface IUserService
{
    Task<bool>              AuthenticateAsync(string username, string password = "");
    Task<bool>              RegisterAsync(string     username, string password = "", string name = "", IEnumerable<Privilege>? privileges = null);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool>              EditAsync(string originalUsername, User updatedUser);
}
