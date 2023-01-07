
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository;
using DataAccess.DataModels;
using System.Collections.Generic;
using Domain.Models;

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

    public async Task<bool> RegisterAsync(string username, string password = "", string name = "", IEnumerable<Privilege>? privileges = null)
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

            if (privileges != null)
            {
                user.UserPrivileges = new List<UserPrivilege>();

                foreach (var privilege in privileges)
                {
                    user.UserPrivileges.Add(new UserPrivilege
                    {
                        UserId = user.Id,
                        Privilege = privilege
                    });
                }
            }

            result = await _repository.Create(user);
        }

        return result;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _repository.GetAllIncluding(includeProperties: u => u.UserPrivileges!);
        return users;
    }
}