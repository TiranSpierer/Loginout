
using DataService.Services;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataService.Initialization;

public class DbInitializor
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public DbInitializor(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async void Initialize()
    {
        string defaultAdminUsername = _configuration["DefaultAdmin:Username"]!;
        string defaultAdminPassword = _configuration["DefaultAdmin:Password"]!;
        var privileges = Enum.GetValues(typeof(Privilege)).Cast<Privilege>();

        // Create the default administrator user if it doesn't already exist
        if (await _userService.AuthenticateAsync(defaultAdminUsername, defaultAdminPassword) == false)
        {
            await _userService.RegisterAsync(defaultAdminUsername, defaultAdminPassword, defaultAdminUsername, privileges);
        }
    }
}
