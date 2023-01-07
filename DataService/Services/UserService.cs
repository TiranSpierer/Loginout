
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository;
using DataAccess.DataModels;

namespace DataService.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IRepository<User> repository, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> AuthenticateAsync(string username, string password = "")
    {
        var user = await _repository.GetById(username);
        bool result = false;

        if (user != null)
        {
            user.Password = string.IsNullOrEmpty(user.Password) ? string.Empty : user.Password;

            result = _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;
        }

        return result;
    }

    public async Task<bool> RegisterAsync(string username, string password = "", string name = "")
    {
        bool result = false;
        if (string.IsNullOrEmpty(username) == false)
        {
            var user = new User
            {
                Id = username,
                Password = _passwordHasher.HashPassword(new User(), password),
                Name = name
            };

            result = await _repository.Create(user);
        }

        return result;
    }
}