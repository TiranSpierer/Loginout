using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DataAccess.Repository;
using DataAccess.DataModels;
using System.Collections.Generic;
using System.Linq;
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
        var user = await _repository.GetByIdAsync(username);
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
        var result = false;
        if (string.IsNullOrEmpty(username) == false)
        {


            var user = new User
            {
                Id = username,
                Password = _passwordHasher.HashPassword(new User(), password),
                Name = name
            };

            user.UserPrivileges = privileges?.Select(privilege => new UserPrivilege
                                                                  {
                                                                      UserId    = user.Id,
                                                                      Privilege = privilege
                                                                  }).ToList();

            result = await _repository.CreateAsync(user);
        }

        return result;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = await _repository.GetAllIncludingPropertiesAsync(includeProperties: u => u.UserPrivileges!);
        return users;
    }

    public async Task<bool> EditAsync(string originalUsername, User updatedUser)
    {
        var isEdited = false;
        var user     = await _repository.GetByIdAsync(originalUsername);

        if (user != null)
        {
            updatedUser.Password = _passwordHasher.HashPassword(new User(), updatedUser.Password!);
            if (originalUsername != updatedUser.Id)
            {
                if (await _repository.GetByIdAsync(updatedUser.Id) == null)
                {
                    await _repository.DeleteAsync(originalUsername);
                    isEdited = await _repository.CreateAsync(updatedUser);
                }
            }
            else
            {
                isEdited = await _repository.UpdateAsync(originalUsername, updatedUser);
            }
        }

        return isEdited;
    }

    public async Task<bool> DeleteAsync(string username)
    {
        return await _repository.DeleteAsync(username);
    }
}