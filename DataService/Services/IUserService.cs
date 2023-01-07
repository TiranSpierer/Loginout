using DataAccess.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataService.Services;

public interface IUserService
{
    Task<bool> AuthenticateAsync(string username, string password = "");
    Task<bool> RegisterAsync(string username, string password = "", string name = "");
    Task<IEnumerable<User>> GetAllUsers();
}
