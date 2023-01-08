using System;
using System.Linq;
using Domain.Models;
using DataService.Services;
using Microsoft.Extensions.Configuration;

namespace DataService.Initialization;

public class DbInitializer
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public DbInitializer(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async void Initialize()
    {
        var defaultAdminUsername = _configuration["DefaultAdmin:Username"]!;
        var defaultAdminPassword = _configuration["DefaultAdmin:Password"]!;
        var privileges = Enum.GetValues(typeof(Privilege)).Cast<Privilege>();

        // Create the default administrator user if it doesn't already exist
        if (await _userService.AuthenticateAsync(defaultAdminUsername, defaultAdminPassword) == false)
        {
            await _userService.RegisterAsync(defaultAdminUsername, defaultAdminPassword, defaultAdminUsername, privileges);
        }
    }
}
